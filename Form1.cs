using System;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace CodeHelper
{
    public partial class Form1 : Form
    {
        bool isRunning = true;
        public Form1()
        {
            InitializeComponent();
            Thread TH = new Thread(KeyboardThread);
            TH.SetApartmentState(ApartmentState.STA);
            CheckForIllegalCrossThreadCalls = false;
            TH.Start();
        }
        private void KeyboardThread() 
        {
            while (isRunning)
            {
                Thread.Sleep(50);
                if ((Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) > 0)
                {
                    if (Check5Press())
                    {
                        Console.WriteLine("Shortcut pressed");
                        string className = Clipboard.GetText();
                        if (className.Contains("operator")) { continue; }
                        string ruleOf5 = string.Empty;
                        ruleOf5 += $"{className}() = default;\n";
                        ruleOf5 += $"~{className}() = default;\n";
                        ruleOf5 += $"{className}(const {className}& other) = delete;\n";
                        ruleOf5 += $"{className}({className}&& other) = delete;\n";
                        ruleOf5 += $"{className}& operator=(const {className}& other) = delete;\n";
                        ruleOf5 += $"{className}& operator=({className}&& other) = delete;";
                        Clipboard.SetText(ruleOf5);
                    }
                }
            }
        }

        private bool Check5Press()
        {
            //QWERTY
            if ((Keyboard.GetKeyStates(Key.D5) & KeyStates.Down) > 0)
            {
                return true;
            }

            //AZERTY
            if (Keyboard.IsKeyDown(Key.D5) && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
            {
                return true;
            }

            return false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            isRunning = false;
        }
    }
}
