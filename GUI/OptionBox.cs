using System;
using System.Collections.Generic;

namespace dge.GUI
{
    public class OptionBox : CheckBox
    {
        List<OptionBox> licked;
        public OptionBox() : base()
        {
            this.licked = new List<OptionBox>();
        }

        public void Link(OptionBox OptionBox)
        {
            OptionBox.ReLink(this);
            this.licked.Add(OptionBox);
            OptionBox.CheckedStateChanged += CheckedChanged;
        }

        internal void ReLink(OptionBox OptionBox)
        {
            //OptionBox.Link(this);
            this.licked.Add(OptionBox);
            OptionBox.CheckedStateChanged += CheckedChanged;
        }

        public void DesLink(OptionBox OptionBox)
        {
            OptionBox.DesLink(this);
            OptionBox.CheckedStateChanged -= CheckedChanged;
        }

        private void CheckedChanged(object sender, CheckedStateChanged e)
        {
            if (((OptionBox)sender).Checked)
            {
                this.b_Checked = false;
            }
        }
    }
}