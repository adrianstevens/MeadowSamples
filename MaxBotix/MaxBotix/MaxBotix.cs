using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Threading.Tasks;

namespace MaxBotix
{
    /* Supports:
     * HRXL-MaxSonar® - WR Series
     * HRLV-MaxSonar® - EZ Series
     * HRLV-ShortRange® - EZ Series
     * HRLV-MaxSonar® - EZ Series
     * IRXL-MaxSonar® - CS Series
     * XL-MaxSonar® - EZ Series
     * XL-MaxSonar® - WR/WRC Series
     * 
     * Not Supported:
     * ParkSonar® - EZ Sensor Series
     * LV-ProxSonar® - EZ Series (parking sensor)
     * I2CXL-MaxSonar® - EZ Series
     */

    public partial class MaxBotix : SensorBase<Length>
    {
        /// <summary>
        /// Raised when the value of the reading changes.
        /// </summary>
        public event EventHandler<IChangeResult<Length>> LengthUpdated = delegate { };

        public Length? Length { get; protected set; }

        Length? serialLength;

        public double VCC { get; set; } = 3.3;

        CommunicationType communication;
        SensorModel sensorModel;

        IAnalogInputPort analogInputPort;

        ISerialMessagePort serialMessagePort;

        public MaxBotix(IAnalogInputPort analogIntputPort,
            SensorModel sensor)
        {
            analogInputPort = analogIntputPort;

            communication = CommunicationType.Analog;
            sensorModel = sensor;

            AnalogInitialize();
        }

        public MaxBotix(IMeadowDevice device, IPin analogIntputPin, 
            SensorModel sensor) : 
            this(device.CreateAnalogInputPort(analogIntputPin), sensor)
        {
        }

        void AnalogInitialize()
        {
            // wire up our observable
            // have to convert from voltage to length units for our consumers
            // this is where the magic is: this allows us to extend the IObservable
            // pattern through the sensor driver
            analogInputPort.Subscribe
            (
                IAnalogInputPort.CreateObserver(
                    async result => {
                        // create a new change result from the new value
                        ChangeResult<Length> changeResult = new ChangeResult<Length>()
                        {
                            New = await ReadSensorAnalog(),
                            Old = Length,
                        };
                        // save state
                        Length = changeResult.New;
                        // notify
                        RaiseEventsAndNotify(changeResult);
                    }
                )
           );
        }

        //The baud rate is 9600, 8 bits, no parity, with one stop bit
        public MaxBotix(IMeadowDevice device, SerialPortName serialPort, SensorModel sensor,
            int serialPortSpeed = 9600)
            : this(device.CreateSerialMessagePort(serialPort, new byte[] { 13 }, false), sensor)
        {
        }

        public MaxBotix(ISerialMessagePort serialMessage, SensorModel sensor)
        {
            serialMessagePort = serialMessage;
            serialMessagePort.MessageReceived += SerialMessagePort_MessageReceived;
            serialMessagePort.Open();

            communication = CommunicationType.Serial;
            sensorModel = sensor;
        }

        private void SerialMessagePort_MessageReceived(object sender, SerialMessageData e)
        {
            Console.WriteLine("Serial message received");

            //R###\n //cm
            //R####\n //mm
            //R####\n //cm
            //need to check inches
            var message = e.GetMessageString(System.Text.Encoding.ASCII);

            if (message[0] != 'R')
                return;

            //it'll throw an exception if it's wrong
            //strip the leading R
            var value = double.Parse(message.Substring(1));

            //need to get this per sensor
            Length.UnitType units = Meadow.Units.Length.UnitType.Millimeters;

            // create a new change result from the new value
            ChangeResult<Length> changeResult = new ChangeResult<Length>()
            {
                New = new Length(value, units),
                Old = Length,
            };
            // save state
            Length = changeResult.New;
            // notify
            RaiseEventsAndNotify(changeResult);
        }

        protected override async Task<Length> ReadSensor()
        {
            return communication switch
            {
                CommunicationType.Analog => await ReadSensorAnalog(),
                CommunicationType.Serial => ReadSensorSerial(),
                _ => throw new NotImplementedException(),
            };
        }

        Length ReadSensorSerial()
        {
            //I think we'll just cache it for serial
            return Length.Value;
        }

        async Task<Length> ReadSensorAnalog()
        {
            var volts = (await analogInputPort.Read()).Volts;
            Length length;

            switch (sensorModel)
            {
                case SensorModel.MB1000:
                case SensorModel.MB1010:
                case SensorModel.MB1020:
                case SensorModel.MB1030:
                case SensorModel.MB1040:
                    //(Vcc/512) per inch
                    length = new Length(volts * 512.0 / VCC, Meadow.Units.Length.UnitType.Inches);
                    break;
                //10m
                case SensorModel.MB1260:
                case SensorModel.MB1261:
                case SensorModel.MB1360:
                case SensorModel.MB1361:
                //16.5m
                case SensorModel.MB2530:
                case SensorModel.MB2532:
                    //(Vcc / 1024) per 2 - cm
                    length = new Length(volts * 2048.0 / VCC, Meadow.Units.Length.UnitType.Centimeters);
                    break;
               
                //5m 
                case SensorModel.MB1003:
                case SensorModel.MB1013:
                case SensorModel.MB1023:
                case SensorModel.MB1033:
                case SensorModel.MB1043:
                //Intentional fall-through
                //5m 
                case SensorModel.MB1004:
                case SensorModel.MB1014:
                case SensorModel.MB1024:
                case SensorModel.MB1034:
                case SensorModel.MB1044:
                //Intentional fall-through 
                //5m HRXL
                case SensorModel.MB7360:
                case SensorModel.MB7367:
                case SensorModel.MB7369:
                case SensorModel.MB7380:
                case SensorModel.MB7387:
                case SensorModel.MB7389:
                    //(Vcc/5120) per 1-mm
                    length = new Length(volts * 5120.0 / VCC, Meadow.Units.Length.UnitType.Millimeters);
                    break;
                //10m HRXL
                case SensorModel.MB7363:
                case SensorModel.MB7366:
                case SensorModel.MB7368:
                case SensorModel.MB7383:
                case SensorModel.MB7386:
                case SensorModel.MB7388:
                //Intentional fall-through
                //7.6m
                case SensorModel.MB1200:
                case SensorModel.MB1210:
                case SensorModel.MB1220:
                case SensorModel.MB1230:
                case SensorModel.MB1240:
                case SensorModel.MB1300:
                case SensorModel.MB1310:
                case SensorModel.MB1320:
                case SensorModel.MB1330:
                case SensorModel.MB1340:
                    //(Vcc / 1024) per 1 - cm
                //1.5m HRXL
                case SensorModel.MB7375:
                case SensorModel.MB7395:
                    //(Vcc / 1024) per 1 - cm
                    length = new Length(volts * 1024.0 / VCC, Meadow.Units.Length.UnitType.Centimeters);
                    break;
                default:
                    length = new Length(0);
                    break;
            }

            return length;
        }

        double ReadSensorPWM()
        {
            throw new NotImplementedException();
        }

        protected override void RaiseEventsAndNotify(IChangeResult<Length> changeResult)
        {
            LengthUpdated?.Invoke(this, changeResult);
            base.RaiseEventsAndNotify(changeResult);
        }

        public void StartUpdating(TimeSpan? updateInterval)
        {
            // thread safety
            lock (samplingLock)
            {
                if (IsSampling) return;
                IsSampling = true;

                if (communication == CommunicationType.Analog)
                {
                    analogInputPort.StartUpdating(updateInterval);
                }
            }
        }

        /// <summary>
        /// Stops sampling the temperature.
        /// </summary>
        public void StopUpdating()
        {
            lock (samplingLock)
            {
                if (!IsSampling) return;
                base.IsSampling = false;

                if (communication == CommunicationType.Analog)
                {
                    analogInputPort.StopUpdating();
                }
            }
        }
    }
}