<Query Kind="Program">
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
</Query>

namespace ScientificCalculator
{
    public class CalculatorForm : Form
    {
        private TextBox displayBox;
        private string currentNumber = "";
        private double storedNumber = 0;
        private string currentOperation = "";
        private bool isNewNumber = true;
        private bool hasDecimalPoint = false;

        public CalculatorForm()
        {
            this.Text = "Scientific Calculator";
            this.Size = new Size(400, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Create display
            displayBox = new TextBox
            {
                Size = new Size(360, 50),
                Location = new Point(10, 10),
                Font = new Font("Arial", 20),
                TextAlign = HorizontalAlignment.Right,
                ReadOnly = true,
                Text = "0"
            };
            this.Controls.Add(displayBox);

            // Create button layout
            CreateButtons();
        }

        private void CreateButtons()
        {
            string[,] buttonLayout = {
                {"MC", "MR", "MS", "M+", "M-"},
                {"←", "CE", "C", "±", "√"},
                {"sin", "cos", "tan", "x²", "1/x"},
                {"7", "8", "9", "/", "%"},
                {"4", "5", "6", "*", "x^y"},
                {"1", "2", "3", "-", "π"},
                {"0", ".", "=", "+", "e"}
            };

            int buttonWidth = 70;
            int buttonHeight = 70;
            int spacing = 5;
            int startX = 10;
            int startY = 70;

            for (int row = 0; row < buttonLayout.GetLength(0); row++)
            {
                for (int col = 0; col < buttonLayout.GetLength(1); col++)
                {
                    Button button = new Button
                    {
                        Text = buttonLayout[row, col],
                        Size = new Size(buttonWidth, buttonHeight),
                        Location = new Point(startX + (buttonWidth + spacing) * col,
                                          startY + (buttonHeight + spacing) * row),
                        Font = new Font("Arial", 12)
                    };
                    button.Click += Button_Click;
                    this.Controls.Add(button);
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string buttonText = button.Text;

            switch (buttonText)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    if (isNewNumber)
                    {
                        displayBox.Text = buttonText;
                        isNewNumber = false;
                    }
                    else
                        displayBox.Text += buttonText;
                    break;

                case ".":
                    if (!hasDecimalPoint)
                    {
                        if (isNewNumber)
                        {
                            displayBox.Text = "0.";
                            isNewNumber = false;
                        }
                        else
                            displayBox.Text += ".";
                        hasDecimalPoint = true;
                    }
                    break;

                case "+":
                case "-":
                case "*":
                case "/":
                    storedNumber = double.Parse(displayBox.Text);
                    currentOperation = buttonText;
                    isNewNumber = true;
                    hasDecimalPoint = false;
                    break;

                case "=":
                    CalculateResult();
                    break;

                case "C":
                    displayBox.Text = "0";
                    currentNumber = "";
                    storedNumber = 0;
                    currentOperation = "";
                    isNewNumber = true;
                    hasDecimalPoint = false;
                    break;

                case "←":
                    if (displayBox.Text.Length > 1)
                        displayBox.Text = displayBox.Text.Substring(0, displayBox.Text.Length - 1);
                    else
                        displayBox.Text = "0";
                    break;

                case "±":
                    displayBox.Text = (-double.Parse(displayBox.Text)).ToString();
                    break;

                case "√":
                    displayBox.Text = Math.Sqrt(double.Parse(displayBox.Text)).ToString();
                    isNewNumber = true;
                    break;

                case "x²":
                    double num = double.Parse(displayBox.Text);
                    displayBox.Text = (num * num).ToString();
                    isNewNumber = true;
                    break;

                case "1/x":
                    displayBox.Text = (1 / double.Parse(displayBox.Text)).ToString();
                    isNewNumber = true;
                    break;

                case "sin":
                    displayBox.Text = Math.Sin(double.Parse(displayBox.Text) * Math.PI / 180).ToString();
                    isNewNumber = true;
                    break;

                case "cos":
                    displayBox.Text = Math.Cos(double.Parse(displayBox.Text) * Math.PI / 180).ToString();
                    isNewNumber = true;
                    break;

                case "tan":
                    displayBox.Text = Math.Tan(double.Parse(displayBox.Text) * Math.PI / 180).ToString();
                    isNewNumber = true;
                    break;

                case "π":
                    displayBox.Text = Math.PI.ToString();
                    isNewNumber = true;
                    break;

                case "e":
                    displayBox.Text = Math.E.ToString();
                    isNewNumber = true;
                    break;
            }
        }

        private void CalculateResult()
        {
            double secondNumber = double.Parse(displayBox.Text);
            double result = currentOperation switch
            {
                "+" => storedNumber + secondNumber,
                "-" => storedNumber - secondNumber,
                "*" => storedNumber * secondNumber,
                "/" => secondNumber != 0 ? storedNumber / secondNumber : double.NaN,
                _ => double.Parse(displayBox.Text)
            };

            displayBox.Text = result.ToString();
            isNewNumber = true;
            hasDecimalPoint = false;
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CalculatorForm());
        }
    }
}