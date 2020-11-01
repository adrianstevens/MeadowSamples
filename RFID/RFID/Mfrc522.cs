using System;
using System.Threading;

namespace RFID
{
    //port from https://github.com/ljos/MFRC522
    public class Mfrc522
    {
        const byte MAX_LEN = 16;        // Maximum length of an array.

        // MF522 MFRC522 error codes.
        const byte MI_OK = 0;         // Everything A-OK.
        const byte MI_NOTAGERR = 1;         // No tag error
        const byte MI_ERR = 2;         // General error

        // MF522 Command word
        const byte MFRC522_IDLE          = 0x00;     // NO action; Cancel the current command
        const byte MFRC522_MEM           = 0x01;     // Store 25 byte into the internal buffer.
        const byte MFRC522_GENID         = 0x02;     // Generates a 10 byte random ID number.
        const byte MFRC522_CALCCRC       = 0x03;     // CRC Calculate or selftest.
        const byte MFRC522_TRANSMIT      = 0x04;     // Transmit data
        const byte MFRC522_NOCMDCH       = 0x07;     // No command change.
        const byte MFRC522_RECEIVE       = 0x08;     // Receive Data
        const byte MFRC522_TRANSCEIVE    = 0x0C;     // Transmit and receive data,
        const byte MFRC522_AUTHENT       = 0x0E;     // Authentication Key
        const byte MFRC522_SOFTRESET = 0x0F;     // Reset

        // Mifare_One tag command word
        const byte MF1_REQIDL            = 0x26;      // find the antenna area does not enter hibernation
        const byte MF1_REQALL            = 0x52;      // find all the tags antenna area
        const byte MF1_ANTICOLL          = 0x93;      // anti-collision
        const byte MF1_SELECTTAG         = 0x93;      // election tag
        const byte MF1_AUTHENT1A         = 0x60;      // authentication key A
        const byte MF1_AUTHENT1B         = 0x61;      // authentication key B
        const byte MF1_READ              = 0x30;      // Read Block
        const byte MF1_WRITE             = 0xA0;      // write block
        const byte MF1_DECREMENT         = 0xC0;      // debit
        const byte MF1_INCREMENT         = 0xC1;      // recharge
        const byte MF1_RESTORE           = 0xC2;      // transfer block data to the buffer
        const byte MF1_TRANSFER          = 0xB0;      // save the data in the buffer
        const byte MF1_HALT = 0x50;      // Sleep


        //------------------ MFRC522 registers---------------
        //Page 0:Command and Status
        const byte Reserved00 = 0x00;
        const byte CommandReg            = 0x01;
        const byte CommIEnReg            = 0x02;
        const byte DivIEnReg             = 0x03;
        const byte CommIrqReg            = 0x04;
        const byte DivIrqReg             = 0x05;
        const byte ErrorReg              = 0x06;
        const byte Status1Reg            = 0x07;
        const byte Status2Reg            = 0x08;
        const byte FIFODataReg           = 0x09;
        const byte FIFOLevelReg          = 0x0A;
        const byte WaterLevelReg         = 0x0B;
        const byte ControlReg            = 0x0C;
        const byte BitFramingReg         = 0x0D;
        const byte CollReg               = 0x0E;
        const byte Reserved01            = 0x0F;
                //Page 1:Command       
        const byte Reserved10            = 0x10;
        const byte ModeReg               = 0x11;
        const byte TxModeReg             = 0x12;
        const byte RxModeReg             = 0x13;
        const byte TxControlReg          = 0x14;
        const byte TxAutoReg             = 0x15;
        const byte TxSelReg              = 0x16;
        const byte RxSelReg              = 0x17;
        const byte RxThresholdReg        = 0x18;
        const byte DemodReg              = 0x19;
        const byte Reserved11            = 0x1A;
        const byte Reserved12            = 0x1B;
        const byte MifareReg             = 0x1C;
        const byte Reserved13            = 0x1D;
        const byte Reserved14            = 0x1E;
        const byte SerialSpeedReg        = 0x1F;
                //Page 2:CFG           
        const byte Reserved20            = 0x20;
        const byte CRCResultRegM         = 0x21;
        const byte CRCResultRegL         = 0x22;
        const byte Reserved21            = 0x23;
        const byte ModWidthReg           = 0x24;
        const byte Reserved22            = 0x25;
        const byte RFCfgReg              = 0x26;
        const byte GsNReg                = 0x27;
        const byte CWGsPReg              = 0x28;
        const byte ModGsPReg             = 0x29;
        const byte TModeReg              = 0x2A;
        const byte TPrescalerReg         = 0x2B;
        const byte TReloadRegH           = 0x2C;
        const byte TReloadRegL           = 0x2D;
        const byte TCounterValueRegH     = 0x2E;
        const byte TCounterValueRegL     = 0x2F;
        //Page 3:TestRegister   
        const byte Reserved30            = 0x30;
        const byte TestSel1Reg           = 0x31;
        const byte TestSel2Reg           = 0x32;
        const byte TestPinEnReg          = 0x33;
        const byte TestPinValueReg       = 0x34;
        const byte TestBusReg            = 0x35;
        const byte AutoTestReg           = 0x36;
        const byte VersionReg            = 0x37;
        const byte AnalogTestReg         = 0x38;
        const byte TestDAC1Reg           = 0x39;
        const byte TestDAC2Reg           = 0x3A;
        const byte TestADCReg            = 0x3B;
        const byte Reserved31            = 0x3C;
        const byte Reserved32            = 0x3D;
        const byte Reserved33            = 0x3E;
        const byte Reserved34            = 0x3F;

        byte _sad = 0; //remove
        bool HIGH = true;
        bool LOW = false;
        void DigitalWrite(byte sad, bool highLow)
        {
            //temp method to get compiling = 
        }


        public Mfrc522()
        {
        }

        void WriteToRegister(byte address, int value)
        {
            WriteToRegister(address, (byte)value);
        }

        void WriteToRegister(byte addr, byte val)
        {
         /*   DigitalWrite(_sad, LOW);

            //Address format: 0XXXXXX0
            SPI.transfer((addr << 1) & 0x7E);
            SPI.transfer(val);

            DigitalWrite(_sad, HIGH); */
        }

        byte ReadFromRegister(int address)
        {
            return ReadFromRegister((byte)address);
        }

        byte ReadFromRegister(byte addr)
        {
            byte val = 0;
          /*  DigitalWrite(_sad, LOW);
            SPI.transfer(((addr << 1) & 0x7E) | 0x80);
            val = SPI.transfer(0x00);
            DigitalWrite(_sad, HIGH); */
            return val; 
        }

        void SetBitMask(byte address, byte mask)
        {
            byte current;
            current = ReadFromRegister(address);
            WriteToRegister(address, (byte)(current | mask));
        }

        /**************************************************************************/
        void ClearBitMask(byte addr, byte mask)
        {
            byte current;
            current = ReadFromRegister(addr);
            WriteToRegister(addr, (byte)(current & (~mask)));
        }

        void Begin()
        {
            DigitalWrite(_sad, HIGH);

            reset();

            //Timer: TPrescaler*TreloadVal/6.78MHz = 24ms
            WriteToRegister(TModeReg, 0x8D);       // Tauto=1; f(Timer) = 6.78MHz/TPreScaler
            WriteToRegister(TPrescalerReg, 0x3E);  // TModeReg[3..0] + TPrescalerReg
            WriteToRegister(TReloadRegL, 30);
            WriteToRegister(TReloadRegH, 0);

            WriteToRegister(TxAutoReg, 0x40);      // 100%ASK
            WriteToRegister(ModeReg, 0x3D);        // CRC initial value 0x6363

            SetBitMask(TxControlReg, 0x03);        // Turn antenna on.
        }

        /**************************************************************************/
        /*!
          @brief   Sends a SOFTRESET command to the MFRC522 chip.
         */
        /**************************************************************************/
        void reset()
        {
            WriteToRegister(CommandReg, MFRC522_SOFTRESET);
        }

        /**************************************************************************/
        /*!
          @brief   Checks the firmware version of the chip.
          @returns The firmware version of the MFRC522 chip.
         */
        /**************************************************************************/
        byte getFirmwareVersion()
        {
            byte response;
            response = ReadFromRegister(VersionReg);
            return response;
        }

        bool digitalSelfTestPass()
        {
            int i;
            byte n;

            byte[] selfTestResultV1 = {0x00, 0xC6, 0x37, 0xD5, 0x32, 0xB7, 0x57, 0x5C,
                          0xC2, 0xD8, 0x7C, 0x4D, 0xD9, 0x70, 0xC7, 0x73,
                          0x10, 0xE6, 0xD2, 0xAA, 0x5E, 0xA1, 0x3E, 0x5A,
                          0x14, 0xAF, 0x30, 0x61, 0xC9, 0x70, 0xDB, 0x2E,
                          0x64, 0x22, 0x72, 0xB5, 0xBD, 0x65, 0xF4, 0xEC,
                          0x22, 0xBC, 0xD3, 0x72, 0x35, 0xCD, 0xAA, 0x41,
                          0x1F, 0xA7, 0xF3, 0x53, 0x14, 0xDE, 0x7E, 0x02,
                          0xD9, 0x0F, 0xB5, 0x5E, 0x25, 0x1D, 0x29, 0x79};
            byte[] selfTestResultV2 = {0x00, 0xEB, 0x66, 0xBA, 0x57, 0xBF, 0x23, 0x95,
                          0xD0, 0xE3, 0x0D, 0x3D, 0x27, 0x89, 0x5C, 0xDE,
                          0x9D, 0x3B, 0xA7, 0x00, 0x21, 0x5B, 0x89, 0x82,
                          0x51, 0x3A, 0xEB, 0x02, 0x0C, 0xA5, 0x00, 0x49,
                          0x7C, 0x84, 0x4D, 0xB3, 0xCC, 0xD2, 0x1B, 0x81,
                          0x5D, 0x48, 0x76, 0xD5, 0x71, 0x61, 0x21, 0xA9,
                          0x86, 0x96, 0x83, 0x38, 0xCF, 0x9D, 0x5B, 0x6D,
                          0xDC, 0x15, 0xBA, 0x3E, 0x7D, 0x95, 0x3B, 0x2F};
            byte[] selfTestResult;

            switch (getFirmwareVersion())
            {
                case 0x91:
                    selfTestResult = selfTestResultV1;
                    break;
                case 0x92:
                    selfTestResult = selfTestResultV2;
                    break;
                default:
                    return false;
            }

            reset();
            WriteToRegister(FIFODataReg, 0x00);
            WriteToRegister(CommandReg, MFRC522_MEM);
            WriteToRegister(AutoTestReg, 0x09);
            WriteToRegister(FIFODataReg, 0x00);
            WriteToRegister(CommandReg, MFRC522_CALCCRC);

            // Wait for the self test to complete.
            i = 0xFF;
            do
            {
                n = ReadFromRegister(DivIrqReg);
                i--;
            }
            while ((i != 0) && (n & 0x04) == 0);
            //while ((i != 0) && !(n & 0x04));

            for (i = 0; i < 64; i++)
            {
                if (ReadFromRegister(FIFODataReg) != selfTestResult[i])
                {
                    //C.println(i);
                    Console.WriteLine($"Read register failed {i}");
                    return false;
                }
            }
            return true;
        }

        int CommandTag(byte cmd, byte[] data, int dlen, byte[] result, ref int rlen)
        {
            int status = MI_ERR;
            byte irqEn = 0x00;
            byte waitIRq = 0x00;
            byte lastBits, n;
            int i;

            switch (cmd)
            {
                case MFRC522_AUTHENT:
                    irqEn = 0x12;
                    waitIRq = 0x10;
                    break;
                case MFRC522_TRANSCEIVE:
                    irqEn = 0x77;
                    waitIRq = 0x30;
                    break;
                default:
                    break;
            }

            WriteToRegister(CommIEnReg, irqEn | 0x80);    // interrupt request
            ClearBitMask(CommIrqReg, 0x80);             // Clear all interrupt requests bits.
            SetBitMask(FIFOLevelReg, 0x80);             // FlushBuffer=1, FIFO initialization.

            WriteToRegister(CommandReg, MFRC522_IDLE);  // No action, cancel the current command.

            // Write to FIFO
            for (i = 0; i < dlen; i++)
            {
                WriteToRegister(FIFODataReg, data[i]);
            }

            // Execute the command.
            WriteToRegister(CommandReg, cmd);
            if (cmd == MFRC522_TRANSCEIVE)
            {
                SetBitMask(BitFramingReg, 0x80);  // StartSend=1, transmission of data starts
            }

            // Waiting for the command to complete so we can receive data.
            i = 25; // Max wait time is 25ms.
            do
            {
                Thread.Sleep(1);
                // CommIRqReg[7..0]
                // Set1 TxIRq RxIRq IdleIRq HiAlerIRq LoAlertIRq ErrIRq TimerIRq
                n = ReadFromRegister(CommIrqReg);
                i--;
            }
            while ((i != 0) &&
            (n & 0x01) ==0 &&
            (n & waitIRq) == 0);
            //while ((i != 0) && !(n & 0x01) && !(n & waitIRq));

            ClearBitMask(BitFramingReg, 0x80);  // StartSend=0

            if (i != 0)
            { // Request did not time out.
                if ((ReadFromRegister(ErrorReg) & 0x1D) == 0)
                {  // BufferOvfl Collerr CRCErr ProtocolErr
                    status = MI_OK;
                    if ( (n & irqEn & 0x01) > 0)
                    {
                        status = MI_NOTAGERR;
                    }

                    if (cmd == MFRC522_TRANSCEIVE)
                    {
                        n = ReadFromRegister(FIFOLevelReg);
                        lastBits = (byte)(ReadFromRegister(ControlReg) & 0x07);
                        if (lastBits > 0)
                        {
                            rlen = (n - 1) * 8 + lastBits;
                        }
                        else
                        {
                            rlen = n * 8;
                        }

                        if (n == 0)
                        {
                            n = 1;
                        }

                        if (n > MAX_LEN)
                        {
                            n = MAX_LEN;
                        }

                        // Reading the recieved data from FIFO.
                        for (i = 0; i < n; i++)
                        {
                            result[i] = ReadFromRegister(FIFODataReg);
                        }
                    }
                }
                else
                {
                    status = MI_ERR;
                }
            }
            return status;
        }

        /**************************************************************************/
        /*!
          @brief   Checks to see if there is a tag in the vicinity.
          @param   mode  The mode we are requsting in.
          @param   type  If we find a tag, this will be the type of that tag.
                         0x4400 = Mifare_UltraLight
                         0x0400 = Mifare_One(S50)
                         0x0200 = Mifare_One(S70)
                         0x0800 = Mifare_Pro(X)
                         0x4403 = Mifare_DESFire
          @returns Returns the status of the request.
                   MI_ERR        if something went wrong,
                   MI_NOTAGERR   if there was no tag to send the command to.
                   MI_OK         if everything went OK.
         */
        /**************************************************************************/
        int RequestTag(byte mode, byte[] data)
        {
            int status;
            int len = 0;
            WriteToRegister(BitFramingReg, 0x07);  // TxLastBists = BitFramingReg[2..0]

            data[0] = mode;
            status = CommandTag(MFRC522_TRANSCEIVE, data, 1, data, ref len);

            if ((status != MI_OK) || (len != 0x10))
            {
                status = MI_ERR;
            }

            return status;
        }

        /**************************************************************************/
        /*!
          @brief   Handles collisions that might occur if there are multiple
                   tags available.
          @param   serial  The serial nb of the tag.
          @returns Returns the status of the collision detection.
                   MI_ERR        if something went wrong,
                   MI_NOTAGERR   if there was no tag to send the command to.
                   MI_OK         if everything went OK.
         */
        /**************************************************************************/
        int AntiCollision(byte[] serial)
        {
            int status, i, len = 0;
            byte check = 0x00;

            WriteToRegister(BitFramingReg, 0x00);  // TxLastBits = BitFramingReg[2..0]

            serial[0] = MF1_ANTICOLL;
            serial[1] = 0x20;
            status = CommandTag(MFRC522_TRANSCEIVE, serial, 2, serial, ref len);
            len = len / 8; // len is in bits, and we want each byte.
            if (status == MI_OK)
            {
                // The checksum of the tag is the ^ of all the values.
                for (i = 0; i < len - 1; i++)
                {
                    check ^= serial[i];
                }
                // The checksum should be the same as the one provided from the
                // tag (serial[4]).
                if (check != serial[i])
                {
                    status = MI_ERR;
                }
            }

            return status;
        }

        /**************************************************************************/
        byte[] CalculateCRC(byte[] data, int len)
        {
            var result = new byte[2];

            int i;
            byte n;

            ClearBitMask(DivIrqReg, 0x04);   // CRCIrq = 0
            SetBitMask(FIFOLevelReg, 0x80);  // Clear the FIFO pointer

            //Writing data to the FIFO.
            for (i = 0; i < len; i++)
            {
                WriteToRegister(FIFODataReg, data[i]);
            }
            WriteToRegister(CommandReg, MFRC522_CALCCRC);

            // Wait for the CRC calculation to complete.
            i = 0xFF;
            do
            {
                n = ReadFromRegister(DivIrqReg);
                i--;
            } while ((i != 0) && (n & 0x04) == 0);  //CRCIrq = 1

            // Read the result from the CRC calculation.
            result[0] = ReadFromRegister(CRCResultRegL);
            result[1] = ReadFromRegister(CRCResultRegM);

            return result;
        }

        /**************************************************************************/
        /*!
          @brief   Selects a tag for processing.
          @param   serial  The serial number of the tag that is to be selected.
          @returns The SAK response from the tag.
         */
        /**************************************************************************/
        byte SelectTag(byte[] serial)
        {
            int i, status, len = 0;
            byte sak;
            byte[] buffer = new byte[9];

            buffer[0] = MF1_SELECTTAG;
            buffer[1] = 0x70;
            for (i = 0; i < 5; i++)
            {
                buffer[i + 2] = serial[i];
            }

            var crc = CalculateCRC(buffer, 7);
            buffer[7] = crc[0];
            buffer[8] = crc[1];

            status = CommandTag(MFRC522_TRANSCEIVE, buffer, 9, buffer, ref len);

            if ((status == MI_OK) && (len == 0x18))
            {
                sak = buffer[0];
            }
            else
            {
                sak = 0;
            }

            return sak;
        }

        /**************************************************************************/
        /*!
          @brief   Handles the authentication between the tag and the reader.
          @param   mode    What authentication key to use.
          @param   block   The block that we want to read.
          @param   key     The authentication key.
          @param   serial  The serial of the tag.
          @returns Returns the status of the collision detection.
                   MI_ERR        if something went wrong,
                   MI_OK         if everything went OK.
         */
        /**************************************************************************/
        int Authenticate(byte mode, byte block, byte[] key, byte[] serial)
        {
            int i, status, len = 0;
            byte[] buffer = new byte[12];

            //Verify the command block address + sector + password + tag serial number
            buffer[0] = mode;          // 0th byte is the mode
            buffer[1] = block;         // 1st byte is the block to address.
            for (i = 0; i < 6; i++)
            {  // 2nd to 7th byte is the authentication key.
                buffer[i + 2] = key[i];
            }
            for (i = 0; i < 4; i++)
            {  // 8th to 11th byte is the serial of the tag.
                buffer[i + 8] = serial[i];
            }

            status = CommandTag(MFRC522_AUTHENT, buffer, 12, buffer, ref len);

            if (status != MI_OK ||
                (ReadFromRegister(Status2Reg) & 0x08) == 0)
            {
                status = MI_ERR;
            }

            return status;
        }

        /**************************************************************************/
        /*!
          @brief   Tries to read from the current (authenticated) tag.
          @param   block   The block that we want to read.
          @param   result  The resulting value returned from the tag.
          @returns Returns the status of the collision detection.
                   MI_ERR        if something went wrong,
                   MI_OK         if everything went OK.
         */
        /**************************************************************************/
        int ReadFromTag(byte block, byte[] result)
        {
            int status, len = 0;

            result[0] = MF1_READ;
            result[1] = block;
            var crc = CalculateCRC(result, 2);
            result[2] = crc[0];
            result[3] = crc[1];

            status = CommandTag(MFRC522_TRANSCEIVE, result, 4, result, ref len);

            if ((status != MI_OK) || (len != 0x90))
            {
                status = MI_ERR;
            }

            return status;
        }

        /**************************************************************************/
        /*!
          @brief   Tries to write to a block on the current tag.
          @param   block  The block that we want to write to.
          @param   data   The data that we shoudl write to the block.
          @returns Returns the status of the collision detection.
                   MI_ERR        if something went wrong,
                   MI_OK         if everything went OK.
         */
        /**************************************************************************/
        int WriteToTag(byte block, byte[] data)
        {
            int status, i, len = 0;
            byte[] buffer = new byte[18];

            buffer[0] = MF1_WRITE;
            buffer[1] = block;
            byte[] crc;
            crc = CalculateCRC(buffer, 2);

            buffer[2] = crc[0];
            buffer[3] = crc[1];

            status = CommandTag(MFRC522_TRANSCEIVE, buffer, 4, buffer, ref len);

            if ((status != MI_OK) || (len != 4) || ((buffer[0] & 0x0F) != 0x0A))
            {
                status = MI_ERR;
            }

            if (status == MI_OK)
            {
                for (i = 0; i < 16; i++)
                {
                    buffer[i] = data[i];
                }
                crc = CalculateCRC(buffer, 16);
                buffer[16] = crc[0];
                buffer[17] = crc[1];

                status = CommandTag(MFRC522_TRANSCEIVE, buffer, 18, buffer, ref len);

                if ((status != MI_OK) || (len != 4) || ((buffer[0] & 0x0F) != 0x0A))
                {
                    status = MI_ERR;
                }
            }

            return status;
        }

        /**************************************************************************/
        /*!
          @brief   Sends a halt command to the current tag.
          @returns Returns the result of the halt.
                   MI_ERR        If the command didn't complete properly.
                   MI_OK         If the command completed.
         */
        /**************************************************************************/
        int haltTag()
        {
            int status, len = 0;
            byte[] buffer = new byte[4];

            buffer[0] = MF1_HALT;
            buffer[1] = 0;

            var crc = CalculateCRC(buffer, 2);
            buffer[2] = crc[0];
            buffer[3] = crc[1];

            status = CommandTag(MFRC522_TRANSCEIVE, buffer, 4, buffer, ref len);
            ClearBitMask(Status2Reg, 0x08);  // turn off encryption
            return status;
        }
    }
}
