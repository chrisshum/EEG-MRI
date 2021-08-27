
using System.IO.Ports;
using System.Threading;
using Microsoft.SPOT;
using SerialInterface;
using System;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.IO;
using System.Text;



namespace MRI
{
    public class EvokedPotentials
    {
        public static int Left = 0;
        public static int Right = 0;
        public static string text = "";
        //text needs to be public for all three different threads to read the current serial port information which is ALWAYS on COM4
        public static int[] ledblink1_1 = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        public static int[] ledblink1_2 = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        public static int[] ledblink2_1 = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        public static int[] ledblink2_2 = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        // off initial set parent varible holding array for led matrix
        public static int[] f1 = new int[8] { 0, 0, 1, 1, 0, 0, 1, 1 };
        public static int[] f2 = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        //flash state
        public static int[] p1 = new int[8] { 1, 0, 1, 0, 1, 0, 1, 0 };
        public static int[] p2 = new int[8] { 0, 1, 0, 1, 0, 1, 0, 1 };
        public static int[] p3 = new int[8] { 1, 0, 0, 1, 1, 0, 0, 1 };
        public static int[] p4 = new int[8] { 0, 1, 1, 0, 0, 1, 1, 0 };
        //pattern state
        public static int[] off = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        // off state
        public static int time = 0;
        public static int time2 = 280;
        public static int time3 = 25;
        public static int counter = 0;
        public static CommPort intext = new CommPort(SerialPorts.COM1, BaudRate.Baudrate9600, Parity.None, 8, StopBits.One);

        // this is COM port of the microcontroller, do not change if you need to change ports change COM port on device manager by searching on start menu


        public static void SerialFVEP()
        {
            var h = 0;

            var data = new OutputPort(Pins.GPIO_PIN_D2, false);

            var clock = new OutputPort(Pins.GPIO_PIN_D3, false);

            var latch = new OutputPort(Pins.GPIO_PIN_D4, false);
            var signal = new OutputPort(Pins.GPIO_PIN_D8, false);
            // runs on 8bit shift register all three pins are set

            while (true)
            {
                if (time == 1)
                {
                    signal.Write(true);
                }

                while (counter <= time3)
                {

                    latch.Write(false);
                    for (int i = 0; i < 8; i++)
                    {
                        Boolean temp = ledblink1_1[i] != 0;
                        // pin writing requires booleen input, != 0 converts int into true or false
                        data.Write(temp);
                        clock.Write(true);
                        Thread.Sleep(1);
                        clock.Write(false);
                    }

                    latch.Write(true);
                    if (time == 100)// 100 is pattern and sends signal from another pin when changes stimulus pattern 
                    {
                        if (h == 1)
                        {
                            signal.Write(false);

                            h = 0;

                        }
                        else
                        {

                            signal.Write(true);
                            h = 1;
                        }
                    }

                    Thread.Sleep(time);

                    latch.Write(false);
                    for (int i = 0; i < 8; i++)
                    {

                        Boolean temp = ledblink1_2[i] != 0;
                        data.Write(temp);
                        clock.Write(true);
                        Thread.Sleep(1);
                        clock.Write(false);
                    }
                    latch.Write(true);

                    Thread.Sleep(time);

                    if (time == 100)
                    {
                        Thread.Sleep(time2);
                    }

                    //}
                    //count = 0;
                    counter++;
                }
                counter = 0;





                if (time == 1)
                {
                    signal.Write(false);
                }
                while (counter <= time3)
                {


                    //while (count < 2)
                    //{
                    //    count++;
                    latch.Write(false);
                    for (int i = 0; i < 8; i++)
                    {

                        Boolean temp = ledblink2_1[i] != 0;
                        data.Write(temp);
                        clock.Write(true);
                        Thread.Sleep(1);
                        clock.Write(false);
                    }

                    latch.Write(true);


                    Thread.Sleep(time);

                    if (time == 100)
                    {
                        if (h == 1)
                        {
                            signal.Write(false);

                            h = 0;

                        }
                        else
                        {

                            signal.Write(true);
                            h = 1;
                        }
                    }


                    latch.Write(false);
                    for (int i = 0; i < 8; i++)
                    {

                        Boolean temp = ledblink2_2[i] != 0;
                        data.Write(temp);
                        clock.Write(true);
                        Thread.Sleep(1);
                        clock.Write(false);
                    }

                    latch.Write(true);


                    Thread.Sleep(time);

                    if (time == 100)
                    {
                        Thread.Sleep(time2);
                    }

                    //}


                    counter++;
                }
                counter = 0;

                //Whole patten for light flash is used for flash and pattern

            }
        }









        public static void BAEP()
        {
            //only 5,6 or 9,10 are capable of pwm varible voltage 
            // found out the hard way but only pin 10 has 32 bit memory for its clock meaning the delay time per click is at max 65536 microseconds with out scaling to milliseconds
            // because the duration of high has to be 100 microsecond 0.1 milliseconds 
            PWM leftEar = new PWM(PWMChannels.PWM_PIN_D10, 125000, 100, PWM.ScaleFactor.Microseconds, false);
            //  PWM rightEar = new PWM(PWMChannels.PWM_PIN_D10, 60000, 100, PWM.ScaleFactor.Microseconds, false);z
            var LRear = new OutputPort(Pins.GPIO_PIN_D9, false);
            //leftEar.Frequency = 20000;
            int j = 1;
            // holder to tell if the system is on or off
            while (true)
            {

                if (text.Length != 0)
                {


                    if (text == "BAEP")
                    {
                        if (j == 0)
                        {

                            leftEar.Stop();
                            j = 1;
                        }
                        else
                        {
                            leftEar.Start();
                            j = 0;
                        }
                        // turns sound off and on

                    }
                    if (text == "Leftear")
                    {
                        LRear.Write(true);
                    }
                    if (text == "Rightear")
                    {
                        LRear.Write(false);
                    }

                    if (text.Substring(0, 3) == "vol")
                    {
                        string Volume = text.Substring(3, 2);

                        double VolPercent = Int64.Parse(Volume);
                        VolPercent = VolPercent / 100000;
                        //  Debug.Print(VolPercent.ToString());

                        double current = leftEar.DutyCycle;

                        leftEar.DutyCycle = VolPercent;
                        leftEar.Stop();
                        //Thread.Sleep(10);
                        leftEar.Start();

                    }
                    if (text == "Volume+")
                    {
                        leftEar.Stop();
                        //rightEar.Stop();
                        leftEar.DutyCycle += 0.00005;
                        //rightEar.DutyCycle += 0.001;
                        leftEar.Start();
                        // rightEar.Start();
                        j = 0;

                    }
                    //increases sound volume
                    if (text == "Volume-")
                    {
                        leftEar.Stop();
                        //rightEar.Stop();
                        // make sure to adust to make it not too muc
                        leftEar.DutyCycle -= 0.00005;

                        //rightEar.DutyCycle -= 0.001;
                        leftEar.Start();
                        //rightEar.Start();
                        j = 0;

                    }

                    // decreases sound volume
                }
                Thread.Sleep(100);
                // time it checks to see what is last button click fast enough that it is not noticible 

            }


        }





        public static void Main()
        {
            Thread AuditoryEP = new Thread(new ThreadStart(BAEP));
            Thread FlashVEP = new Thread(new ThreadStart(SerialFVEP));
            FlashVEP.Start();
            AuditoryEP.Start();
            // start both threads to begin with but both are defaulted to off

            // place holder again for on or off state
            var leftshock = new OutputPort(Pins.GPIO_PIN_D6, true);
            var rightshock = new OutputPort(Pins.GPIO_PIN_D7, true);
            while (true)
            {
                text = intext.ReadLine();

                if (text.Length != 0)
                {
                    Debug.Print(text);
                    if (text == "leftShockon")
                    {
                        leftshock.Write(false);
                    }
                    if (text == "rightShockon")
                    {
                        rightshock.Write(false);

                    }
                    if (text == "leftShockoff")
                    {
                        leftshock.Write(true);
                    }
                    if (text == "rightShockoff")
                    {
                        rightshock.Write(true);

                    }
                    if (text.Substring(0, 3) == "FHz" && text.Length >= 4)
                    {
                        time2 = Int32.Parse(text.Substring(3, text.Length - 3));

                    }
                    if (text.Substring(0, 3) == "PHz" && text.Length >= 4)
                    {

                        time3 = Int32.Parse(text.Substring(3, text.Length - 3));
                        time3 = time3/20;
                    }


                    if (text == "Leftvisual")
                    {
                        if (Left == 1)
                        {
                            Left = 0;
                            if (time == 100)
                            {
                                for (int i = 0; i < 8; i++)
                                {
                                    ledblink1_1[i] = f1[i];
                                    ledblink1_2[i] = f2[i];
                                    ledblink2_1[i] = f1[i];
                                    ledblink2_2[i] = f2[i];
                                }
                            }
                            if (time == 2)
                            {
                                for (int i = 0; i < 8; i++)
                                {
                                    ledblink1_1[i] = p1[i];
                                    ledblink1_2[i] = p2[i];
                                    ledblink2_1[i] = p3[i];
                                    ledblink2_2[i] = p4[i];

                                }
                            }
                        }
                        if (Left == 0)
                        {
                            Left = 1;
                            for (int i = 4; i < 8; i++)
                            {

                                ledblink1_1[i] = 0;
                                ledblink1_2[i] = 0;
                                ledblink2_1[i] = 0;
                                ledblink2_2[i] = 0;
                            }

                        }



                    }
                    if (text == "Rightvisual")
                    {
                        if (Right == 1)
                        {
                            Right = 0;
                            if (time == 100)
                            {
                                for (int i = 0; i < 8; i++)
                                {
                                    ledblink1_1[i] = f1[i];
                                    ledblink1_2[i] = f2[i];
                                    ledblink2_1[i] = f1[i];
                                    ledblink2_2[i] = f2[i];
                                }
                            }
                            if (time == 2)
                            {
                                for (int i = 0; i < 8; i++)
                                {
                                    ledblink1_1[i] = p1[i];
                                    ledblink1_2[i] = p2[i];
                                    ledblink2_1[i] = p3[i];
                                    ledblink2_2[i] = p4[i];

                                }
                            }
                        }

                        if (Right == 0)
                        {
                            Right = 1;
                            for (int i = 0; i < 4; i++)
                            {

                                ledblink1_1[i] = 0;
                                ledblink1_2[i] = 0;
                                ledblink2_1[i] = 0;
                                ledblink2_2[i] = 0;
                            }

                        }


                    }

                    if (text == "FLED")
                    {
                        time = 100;
                        for (int i = 0; i < 8; i++)
                        {
                            ledblink1_1[i] = f1[i];
                            ledblink1_2[i] = f2[i];
                            ledblink2_1[i] = f1[i];
                            ledblink2_2[i] = f2[i];
                        }





                    }
                    // all above is to load the pinstatus of leds when flashing and it has an off or on state


                }
                if (text == "PLED")
                {

                    time = 1;
                    for (int i = 0; i < 8; i++)
                    {
                        ledblink1_1[i] = p1[i];
                        ledblink1_2[i] = p2[i];
                        ledblink2_1[i] = p3[i];
                        ledblink2_2[i] = p4[i];

                    }


                }
                if (text == "VisOff")
                {
                    time = 800;
                    for (int i = 0; i < 8; i++)
                    {
                        ledblink1_1[i] = p1[0];
                        ledblink1_2[i] = p2[0];
                        ledblink2_1[i] = p3[0];
                        ledblink2_2[i] = p4[0];
                    }


                }


                Thread.Sleep(100);

            }






        }





    }

}

