using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Herring
{
    class ChartView: PictureBox
    {
        Chart chart = new Chart();

        int? hoveredBar;
                
        // IN 5 min quants
        int selectionStart = -1;
        int selectionEnd = -1;
        int start = -1;

        Log log;

        /// <summary>
        /// Informs about changed seletion (start or span).
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Returns only minutes/hours
        /// </summary>
        public DateTime? SelectionStart
        {
            get
            {
                if (selectionStart == -1)
                    return null;

                
                var span = new TimeSpan(0, (selectionStart / 4) * 5, 0);
                var dateTime = new DateTime() + span;

                return dateTime;
            }

        }

        public TimeSpan? SelectionSpan
        {
            get
            {
                if (selectionStart == -1 || selectionEnd == -1)
                    return null;

                var span = new TimeSpan(0, ((selectionEnd - selectionStart)/4) *5, 0);

                return span;
            }

        }

        [Browsable(false)]
        public int? SelectedBar
        {
            get
            {
                return hoveredBar;
            }

            set
            {
                if (hoveredBar == value)
                    return;

                hoveredBar = value;

                RepaintLog();
            }
        }


        public DateTime? SelectedTime
        {
            get
            {
                if (SelectedBar == null)
                    return null;

                return new DateTime().AddMinutes(SelectedBar.Value * 5);

            }
        }

        public ChartView()
        {
            Init();
        }

        public void UpdateChart(ActivitySummary summary)
        {
            chart.UpdateChart(summary);
            Invalidate();
        }

        public void RefreshChart(Log items)
        {
            log = items;
            RepaintLog();
        }

        private void RepaintLog()
        {
            chart.CreateChart(log, hoveredBar, selectionStart, selectionEnd);
            Image = chart.Bitmap;
        }

        private void Init()
        {
            BorderStyle = BorderStyle.Fixed3D;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var x = Math.Min(Math.Max(0, e.X), 12 * 24 * 4);
                start = (x / 4) * 4;
                selectionStart = -1;
                selectionEnd = -1;

                OnSelectionChanged();
                RepaintLog();
            }
            else
            {
                var newSelection = e.X / 4;

                if (newSelection == hoveredBar)
                    return;

                hoveredBar = newSelection;

                RepaintLog();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Math.Abs(selectionStart - selectionEnd) < 5)
                {
                    selectionStart = -1;
                    selectionEnd = -1;
                    start = -1;

                    OnSelectionChanged();

                    RepaintLog();
                }
            }
            else
            {
                var newSelection = e.X / 4;

                if (newSelection == hoveredBar)
                    return;

                hoveredBar = newSelection;

                RepaintLog();
            }

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var x = Math.Min(Math.Max(0, e.X), 12 * 24 * 4);

            if (e.Button == MouseButtons.Left && Math.Abs(start - x) >= 5) // Draging
            {
     
                selectionEnd =   Math.Max(start, x);
                selectionStart = Math.Min(start, x);

                OnSelectionChanged();

                hoveredBar = null;

                RepaintLog();

                return;
            }

            if (e.Button == MouseButtons.None)
            {
                var newSelection = e.X / 4;

                if (newSelection == hoveredBar)
                    return;

                hoveredBar = newSelection;

                RepaintLog();
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {

            base.OnMouseLeave(e);
        }

        private void OnSelectionChanged()
        {
            if (SelectionChanged != null)
                SelectionChanged(this, EventArgs.Empty);
        }
    }
}
