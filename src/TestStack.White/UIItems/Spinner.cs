using System.Windows.Automation;
using TestStack.White.AutomationElementSearch;
using TestStack.White.UIItems.Actions;

namespace TestStack.White.UIItems
{
    public class Spinner : UIItem
    {
        private readonly AutomationElementFinder finder;
        protected Spinner() {}

        public Spinner(AutomationElement automationElement, ActionListener actionListener) : base(automationElement, actionListener)
        {
            finder = new AutomationElementFinder(automationElement);
        }

        public virtual double Value
        {
            get
            {
                try
                {
                    ValuePattern valuePattern = GetValuePattern ();
                    string value = valuePattern.Current.Value;
                    return double.Parse ( value );
                }
                catch
                {
                    RangeValuePattern rangeValuePattern = ( RangeValuePattern ) Pattern ( RangeValuePattern.Pattern );
                    return rangeValuePattern.Current.Value;
                }
            }
            set
            {
                try
                {
                    GetValuePattern ().SetValue ( value.ToString () );
                }
                catch
                {
                    RangeValuePattern rangeValuePattern = ( RangeValuePattern ) Pattern ( RangeValuePattern.Pattern );
                    rangeValuePattern.SetValue ( value );
                }
            }
        }

        private ValuePattern GetValuePattern()
        {
            AutomationElement spinnerElementContainingValue =
                finder.FindChildRaw(AutomationSearchCondition.ByAutomationId(automationElement.Current.AutomationId).OfControlType(ControlType.Spinner));
            if (spinnerElementContainingValue == null) throw new WhiteAssertionException("Could not find Raw Spinner Element containing the value");
            return (ValuePattern) spinnerElementContainingValue.GetCurrentPattern(ValuePattern.Pattern);
        }

        public virtual void Increment()
        {
            Button button = GetButton("SmallIncrement");
            button.Click();
        }

        private Button GetButton(string buttonName)
        {
            return new Button(finder.Child(AutomationSearchCondition.ByControlType(ControlType.Button).WithAutomationId(buttonName)), actionListener);
        }

        public virtual void Decrement()
        {
            GetButton("SmallDecrement").Click();
        }
    }
}