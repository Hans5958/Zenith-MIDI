﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptedEngine
{
    public abstract class UISetting
    {
        public double Padding { get; set; } = 10;
    }
    public abstract class UISettingEnabled : UISetting
    {
        private bool enabled = true;

        public event Action<bool> EnableToggled;

        public bool Enabled
        {
            get => enabled;
            set
            {
                if (enabled != value)
                {
                    enabled = value;
                    EnableToggled?.Invoke(Enabled);
                }
            }
        }

    }

    public class UITabs : UISetting
    {
        public Dictionary<string, IEnumerable<UISetting>> Tabs { get; set; } = new Dictionary<string, IEnumerable<UISetting>>();

        public UITabs() { }
        public UITabs(Dictionary<string, IEnumerable<UISetting>> tabs) { Tabs = tabs; }
    }

    public class UILabel : UISetting
    {
        public double FontSize { get; } = 16;
        public string Text { get; } = "";

        public UILabel(string text) { Text = text; }
        public UILabel(string text, double fontSize) { Text = text; FontSize = fontSize; }
        public UILabel(string text, double fontSize, double padding) { Text = text; FontSize = fontSize; Padding = padding; }
    }

    public class UINumber : UISettingEnabled
    {
        private double value;

        public UINumber(string text, double value, double minimum, double maximum, int decialPoints)
        {
            Text = text;
            Value = value;
            Minimum = minimum;
            Maximum = maximum;
            DecialPoints = decialPoints;
        }

        public UINumber(string text, double value, double minimum, double maximum, int decialPoints, double step) : this(text, value, minimum, maximum, decialPoints)
        {
            Step = step;
        }

        public event Action<double> ValueChanged;

        public double Value
        {
            get => value;
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    ValueChanged?.Invoke(this.value);
                }
            }
        }
        public string Text { get; }
        public double Minimum { get; }
        public double Maximum { get; }
        public int DecialPoints { get; }
        public double Step { get; } = 1;
    }

    public class UINumberSlider : UISettingEnabled
    {
        private double value;

        public UINumberSlider(string text, double value, double minimum, double maximum, double trueMinimum, double trueMaximum, int decialPoints)
        {
            Text = text;
            Value = value;
            Minimum = minimum;
            Maximum = maximum;
            TrueMinimum = trueMinimum;
            TrueMaximum = trueMaximum;
            DecialPoints = decialPoints;
        }

        public UINumberSlider(string text, double value, double minimum, double maximum, double trueMinimum, double trueMaximum, int decialPoints, bool logarithmic) : this(text, value, minimum, maximum, trueMinimum, trueMaximum, decialPoints)
        {
            Logarithmic = logarithmic;
        }


        public event Action<double> ValueChanged;

        public double Value
        {
            get => value;
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    ValueChanged?.Invoke(this.value);
                }
            }
        }
        public string Text { get; }
        public double Minimum { get; }
        public double Maximum { get; }
        public double TrueMinimum { get; }
        public double TrueMaximum { get; }
        public int DecialPoints { get; }
        public bool Logarithmic { get; } = false;
    }

    public class UIDropdown : UISettingEnabled
    {
        private string value;
        private int index;

        public UIDropdown(string text, string[] options)
        {
            Text = text;
            Options = options;
            Index = 0;
            Value = options[0];
        }

        public UIDropdown(string text, int index, string[] options)
        {
            Text = text;
            Index = index;
            Value = options[index];
            Options = options;
        }


        public event Action<int> IndexChanged;

        public string Value
        {
            get => value;
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    if (!Options.Contains(value)) throw new Exception("Dropdown doesn't contain value '" + value + "'");
                    Index = Array.IndexOf(Options, value);
                }
            }
        }
        public string Text { get; }
        public int Index
        {
            get => index;
            set
            {
                if (index != value)
                {
                    index = value;
                    if (index < 0 || index >= Options.Length) throw new Exception("Index value of " + index + " is outside the rage of the dropdown's " + Options.Length + " item count.");
                    this.value = Options[index];
                    IndexChanged?.Invoke(index);
                }
            }
        }
        public string[] Options { get; }
    }

    public class UICheckbox : UISettingEnabled
    {
        private bool value;

        public UICheckbox(string text, bool @checked)
        {
            this.Text = text;
            this.Checked = @checked;
        }



        public event Action<bool> ValueChanged;

        public bool Checked
        {
            get => value;
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    ValueChanged?.Invoke(this.value);
                }
            }
        }
        public string Text { get; }
    }
}
