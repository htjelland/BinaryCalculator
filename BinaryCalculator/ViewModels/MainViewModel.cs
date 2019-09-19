using BinaryCalculator.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BinaryCalculator.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        //Property for display of inputs and calculated results
        private string _display;
        public string Display
        {
            get => _display;
            set => SetProperty(ref _display, value);
        }

        private string _operand; //Latest input binary number (one or more digits 0 or 1)
        private string _operator; // Latest input operator + or -
        private int _result; //Calculated result (until now)
        private ICalculator _calculator; //Unity injected dependency class
        private CommandType _lastCommand; //Keep track of latest command executed

        public enum CommandType //List of possible button commands
        {
            Undefined = 0,
            Numeric = 1,
            Operator = 2,
            Result = 3,
            Clear = 4,
            ClearEntry = 5,
            Backspace = 6,
        }
        public DelegateCommand<string> NumericCommand { get; set; }
        public DelegateCommand<string> OperatorCommand { get; set; }
        public DelegateCommand ResultCommand { get; set; }
        public DelegateCommand ClearCommand { get; set; }
        public DelegateCommand ClearEntryCommand { get; set; }
        public DelegateCommand BackspaceCommand { get; set; }

        //Constructor
        public MainViewModel(ICalculator calculator)
        {
            _calculator = calculator;

            NumericCommand = new DelegateCommand<string>((s) => AddNumber(s));
            OperatorCommand = new DelegateCommand<string>((s) => AddOperator(s));
            ResultCommand = new DelegateCommand(() => ShowResult());
            ClearCommand = new DelegateCommand(() => Clear());
            ClearEntryCommand = new DelegateCommand(() => ClearEntry());
            BackspaceCommand = new DelegateCommand(() => Backspace(), //Execute
                                                   () => !string.IsNullOrEmpty(Display)) //CanExecute
                                                   .ObservesProperty(() => Display);
        }

        private void AddNumber(string input)
        {
            if(_lastCommand == CommandType.Result) //A numeric button '#' is pressed after '=' indicating a new calculation
            {
                //Reset
                _operand = string.Empty;
                _operator = "+";
                _result = 0;
            }

            //Receive and display numeric input
            _operand += input;
            Display = BinaryFormat(_operand);

            //Store command
            _lastCommand = CommandType.Numeric;
        }
        
        private void AddOperator(string input)
        {
            if(_lastCommand != CommandType.Result) //Unless an operator button '+/-' is pressed directly after '=' indicating a continued calculation
            {
                //Calculate result until now
                if(!string.IsNullOrEmpty(_operand))
                {
                    var operand = BinaryStringToInt(_operand);
                    if ( operand > -1)  //Successful conversion
                    {
                        _result = Calculate(_result, operand, _operator);
                    }
                    else
                    {
                        Display = "Error";
                    }
                }
            }

            //Receive operator input
            _operator = input;

            //Reset
            _operand = string.Empty;

            //Store command
            _lastCommand = CommandType.Operator;
        }

        private void ShowResult()
        {
            //Calculate result
            if (!string.IsNullOrEmpty(_operand))
            {
                var operand = BinaryStringToInt(_operand);
                if (operand > -1)  //Successful conversion
                {
                    _result = Calculate(_result, operand, _operator);
                }
                else
                {
                    Display = "Error";
                }
            }

            //Display result
            Display = BinaryFormat(Convert.ToString(_result, 2));

            //Store command
            _lastCommand = CommandType.Result;
        }

        private int Calculate(int result, int operand, string operation)
        {
            return _calculator.Calculate(result, operand, operation);
        }

        private void Clear()
        {
            //Reset
            _operand = string.Empty;
            _operator = "+";
            _result = 0;

            //Clear display
            Display = string.Empty;

            //Store command
            _lastCommand = CommandType.Clear;
        }

        private void ClearEntry()
        {
            //Reset
            _operand = string.Empty;

            //Clear display
            Display = string.Empty;

            //Store command
            _lastCommand = CommandType.ClearEntry;
        }

        private void Backspace()
        {
            if (_lastCommand == CommandType.Numeric || _lastCommand == CommandType.Backspace) //Backspace is pressed after a numeric button '#' or a previous Backspace
            {
                //Remove last digit of numeric input
                if(_operand.Length > 0)
                {
                    _operand = _operand.Remove(_operand.Length - 1, 1);
                    Display = BinaryFormat(_operand);
                }

                //Store command
                _lastCommand = CommandType.Backspace;
            }
        }

        //Format binary string to groups of up to 4 digits separated by space
        private string BinaryFormat(string source)
        {
            return Regex.Replace(source, "[01]{4}", "$0 ");
        }

        //Convert binary string to integer
        private int BinaryStringToInt(string source)
        {
            int value;
            try
            {
                value = Convert.ToInt32(source, 2);
            }
            catch
            {
                //Return error
                value = -1;
            }
            return value;
        }
    }
}
