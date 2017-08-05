using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Herring
{
    class ChartView: PictureBox
    {
        Chart chart = new Chart();

        int hoveredBar = 0;
                
        // IN 5 min quants
        int selectionStart = -1;
        int selectionEnd = -1;
        
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

        public int SelectedBar
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
            selectionStart = (e.X / 4) * 4;
            selectionEnd = (e.X / 4) * 4;

            OnSelectionChanged();

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

                    OnSelectionChanged();

                    RepaintLog();
                }
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Math.Abs(selectionStart - e.X) >= 5) // Draging
            {
                var x = Math.Min(Math.Max(0, e.X), 12 * 24 * 4);

                selectionStart = Math.Min(selectionStart, x);
                selectionEnd =   Math.Max(selectionEnd, x);
                
                OnSelectionChanged();

                hoveredBar = -1;

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
            if (hoveredBar != -1)
            {
                hoveredBar = -1;
                RepaintLog();
            }

            base.OnMouseLeave(e);
        }

        private void OnSelectionChanged()
        {
            if (SelectionChanged != null)
                SelectionChanged(this, EventArgs.Empty);
        }
    }
}
