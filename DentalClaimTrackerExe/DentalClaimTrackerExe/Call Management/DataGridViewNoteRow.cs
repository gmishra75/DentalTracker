using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace C_DentalClaimTracker
{
    public class DataGridViewNoteRow : DataGridViewRow
    {
        private notes _note;

        public DataGridViewNoteRow() : base() { }

        public notes Note
        {
            get { return _note; }
            set { _note = value; }
        }
    }
}
