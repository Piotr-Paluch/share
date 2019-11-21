using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Conveyor.Controls
{
    public interface IContainerSlot
    {
        SlotState SlotState { get; set; }
    }

    public abstract class ContainerSlotBase 
        : IContainerSlot, 
        INotifyPropertyChanged
    {
        SlotState slotState = SlotState.Free;

        public event PropertyChangedEventHandler PropertyChanged;

        public SlotState SlotState
        {
            get { return slotState; }
            set
            {
                if (slotState == value) return;
                slotState = value;
                OnPropertyChanged();

                Update();
            }
        }

        protected virtual void Update() { }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ContainerSlot : ContainerSlotBase
    {
        public Brush FreeColor { get; set; } = Brushes.White;

        Brush background;

        public ContainerSlot()
        {
            background = FreeColor;
        }

        public Brush Background
        {
            get { return background; }
            set
            {
                background = value;
                OnPropertyChanged();
            }
        }

        public int SlotStateId => (int)SlotState;

        protected override void Update()
        {
            switch (SlotState)
            {
                case SlotState.Free:
                    Background = FreeColor;
                    break;
                case SlotState.Reserved:
                    Background = Brushes.Orange;
                    break;
                case SlotState.Occupied:
                    Background = Brushes.Red;
                    break;

                default: throw new NotSupportedException();
            }
            OnPropertyChanged(nameof(SlotStateId));
        }
    }

    public enum SlotState
    {
        Free = 0,
        Reserved = 1,
        Occupied = 2,
    }

    /// <summary>
    /// Interaction logic for PBNodeBuffer.xaml
    /// </summary>
    public partial class PBNodeBuffer : UserControl
    {

        public ObservableCollection<IContainerSlot> ContainerItems
        {
            get
            {
                return (ObservableCollection<IContainerSlot>)GetValue(ContainerItemsProperty);
            }
            set
            {
                SetValue(ContainerItemsProperty, value);
            }
        }
        public static readonly DependencyProperty ContainerItemsProperty =
            DependencyProperty.Register(nameof(ContainerItems), typeof(ObservableCollection<IContainerSlot>), typeof(PBNodeBuffer), new PropertyMetadata());

        public PBNodeBuffer()
        {
            InitializeComponent();
            //DataContext = this;
            ContainerItems = new ObservableCollection<IContainerSlot>();
            //BufferSize = 9;
        }

        public void UpdateView_OLD()
        {
            int current = ContainerItems.Count - 1;


            // InBuffer - Occupied
            var temp = InBuffer;
            while (current != -1 && temp != 0)
            {
                ContainerItems[current].SlotState = SlotState.Occupied;
                current--;
                temp--;                
            }

            // CurrentDestination - Reserved
            temp = ReservedBuffer;
            while (current != -1 && temp != 0)
            {
                ContainerItems[current].SlotState = SlotState.Reserved;
                current--;
                temp--;
            }

            // Empty - Free
            while (current != -1)
            {
                if(ContainerItems[current].SlotState == SlotState.Free)
                {
                    break;
                }
                ContainerItems[current].SlotState = SlotState.Free;
                current--;
            }

            IsOverflow();
        }
        public void UpdateView()
        {
            int current = 0;
            int last = ContainerItems.Count;


            // InBuffer - Occupied
            var temp = InBuffer;
            while (current != last && temp != 0)
            {
                ContainerItems[current].SlotState = SlotState.Occupied;
                current++;
                temp--;
            }

            // CurrentDestination - Reserved
            temp = ReservedBuffer;
            while (current != last && temp != 0)
            {
                ContainerItems[current].SlotState = SlotState.Reserved;
                current++;
                temp--;
            }

            // Empty - Free
            while (current != last)
            {
                if (ContainerItems[current].SlotState == SlotState.Free)
                {
                    break;
                }
                ContainerItems[current].SlotState = SlotState.Free;
                current++;
            }

            IsOverflow();
        }

        void IsOverflow()
        {
            if (InBuffer >= BufferSize)
            {
                //if(imgOverflow.Visibility != Visibility.Visible) imgOverflow.Visibility = Visibility.Visible;
            }
            else
            {
                //if (imgOverflow.Visibility != Visibility.Hidden) imgOverflow.Visibility = Visibility.Hidden;
            }
        }






        public int BufferSize
        {
            get { return (int)GetValue(BufferSizeProperty); }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(BufferSize));
                SetValue(BufferSizeProperty, value);
            }
        }


        public int InBuffer
        {
            get { return (int)GetValue(InBufferProperty); }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(InBuffer));               
                SetValue(InBufferProperty, value);
            }
        }

        public int ReservedBuffer
        {
            get { return (int)GetValue(ReservedBufferProperty); }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(ReservedBuffer));
                SetValue(ReservedBufferProperty, value);
            }
        }



        public double SlotHeight => base.Height / this.BufferSize;
        public bool IsFull => this.InBuffer >= this.BufferSize;




        void BufferSizeChanged()
        {
            ContainerItems.Clear();
            for (int i = 0; i < BufferSize; i++)
            {
                ContainerItems.Add(new ContainerSlot());
            }

            //Height = lbContainers.Height;
            //Width = lbContainers.Width;
        }










        public static readonly DependencyProperty BufferSizeProperty =
            DependencyProperty.Register(nameof(BufferSize), typeof(int), typeof(PBNodeBuffer), new PropertyMetadata(0, BufferSizePropertyChanged));

        public static readonly DependencyProperty InBufferProperty =
            DependencyProperty.Register(nameof(InBuffer), typeof(int), typeof(PBNodeBuffer), new PropertyMetadata(0, InBufferPropertyChanged));

        public static readonly DependencyProperty ReservedBufferProperty =
            DependencyProperty.Register(nameof(ReservedBuffer), typeof(int), typeof(PBNodeBuffer), new PropertyMetadata(0, ReservedBufferPropertyChanged));

        static void BufferSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pb = (PBNodeBuffer)d;

            if ((int)e.NewValue == 0)
            {
                return;
            }

            pb.ContainerItems = new ObservableCollection<IContainerSlot>();
            pb.BufferSizeChanged();
            pb.UpdateInBufferPercent();
            //Debug.Write($"Width={(int)pb.Width} Height={(int)pb.Height} slotHeight={(int)pb.SlotHeight}");
        }

        static void ReservedBufferPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pb = (PBNodeBuffer)d;
            Debug.WriteLine($"pb = {pb.ReservedBuffer}");
            pb.UpdateView();
        }

        static void InBufferPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pb = (PBNodeBuffer)d;
            pb.UpdateView();
            pb.UpdateInBufferPercent();
        }













        public static readonly DependencyProperty InBufferPercentProperty =
            DependencyProperty.Register(nameof(InBufferPercent), typeof(double), typeof(PBNodeBuffer), new PropertyMetadata(0.0));

        public double InBufferPercent
        {
            get { return (double)GetValue(InBufferPercentProperty); }
            set { SetValue(InBufferPercentProperty, value); }
        }

        void UpdateInBufferPercent()
        {
            if (BufferSize == 0)
            {
                InBufferPercent = 0;
                return;
            }
            if(InBuffer >= BufferSize)
            {
                InBufferPercent = 1;
                return;
            }
            InBufferPercent = (((double)InBuffer / BufferSize));
        }


        #region DependencyProperty

        #endregion
    }
}
