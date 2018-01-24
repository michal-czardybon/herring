using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Herring
{
   internal class ChartView : PictureBox
   {
      public class Bar
      {
         private int minutes;

         private Bar(int minutes)
         {
            this.minutes = minutes;
         }

         public static Bar FromBar(int bar)
         {
            return new Bar(bar*5);
         }

         public static Bar FromMinutes(int minutes)
         {
            return new Bar(minutes);
         }

         public int Minutes
         {
            get => minutes;
            set => minutes = value;
         }

         public int Number
         {
            get => Minutes / 5;
            set => minutes = value * 5;
         }

         public TimeSpan Span => TimeSpan.FromMinutes(minutes);
      }

      private readonly Chart chart = new Chart();

      private Bar hoveredBar;

      private readonly int minutesInBar = 5;
      private readonly int pixelsPerBar = 4;

      // IN 5 min quants
      private int selectionStart = -1;
      private int selectionEnd = -1;
      private int start = -1;

      Log log;

      private readonly int BarsInDay = 24 * 12;

      private int ScreenToBar(int screen)
      {
         return screen / pixelsPerBar;
      }

      private int BarToScreen(int bar)
      {
         return bar * pixelsPerBar;
      }

      private int BarToMinutes(int bar)
      {
         return bar * minutesInBar;
      }

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

            var span = new TimeSpan(0, ScreenToBar(selectionStart) * 5, 0);
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

            var range = Math.Max(0, selectionEnd - selectionStart);
            var span = new TimeSpan(0, ScreenToBar(range) * 5, 0);

            return span;
         }

      }

      [Browsable(false)]
      public Bar SelectedBar
      {
         get => hoveredBar;

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
         chart.CreateChart(log, hoveredBar?.Number, selectionStart, selectionEnd);
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
            var x = Math.Min(Math.Max(0, e.X), BarToScreen(BarsInDay));

            start = (x / pixelsPerBar) * pixelsPerBar;

            selectionStart = -1;
            selectionEnd = -1;

            OnSelectionChanged();
            RepaintLog();
         }
         else
         {
            var newSelection = Bar.FromBar(ScreenToBar(e.X));

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
            var newSelection =  Bar.FromBar( Math.Max(BarsInDay, ScreenToBar(e.X)));

            if (newSelection == hoveredBar)
               return;

            hoveredBar = newSelection;

            RepaintLog();
         }

         base.OnMouseUp(e);
      }

      protected override void OnMouseMove(MouseEventArgs e)
      {
         var x = Math.Min(Math.Max(0, e.X), BarToScreen(BarsInDay));
         
         var newSelection =  Bar.FromBar(Math.Min(BarsInDay, ScreenToBar(e.X)));

         if (e.Button == MouseButtons.Left && Math.Abs(start - x) >= 5) // Draging
         {

            selectionEnd = Math.Max(start, x);
            selectionStart = Math.Min(start, x);

            OnSelectionChanged();

            hoveredBar = newSelection;

            RepaintLog();

            return;
         }

         if (e.Button == MouseButtons.None)
         {
            

            if (newSelection == hoveredBar)
               return;

            hoveredBar = newSelection;

            RepaintLog();
         }

         base.OnMouseMove(e);
      }

      private void OnSelectionChanged()
      {
         SelectionChanged?.Invoke(this, EventArgs.Empty);
      }
   }
}
