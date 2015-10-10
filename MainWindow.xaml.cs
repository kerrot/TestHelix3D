using System;
using System.Collections.Generic;
using System.Linq;
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
using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;
using System.Collections.ObjectModel;

namespace TestHelix3D
{
    

    public class TruncatedConeNode : TruncatedConeVisual3D, ModelNode
    {
        private TranslateTransform3D ChildNode = new TranslateTransform3D();

        protected override MeshGeometry3D Tessellate()
        {
            ChildNode.OffsetY = Height;
            RotateTransform3D d;
            
            return base.Tessellate();
        }

        Transform3D ModelNode.GetTT()
        {
            return ChildNode;
        }

        void ModelNode.SetTT(Transform3D a_TT)
        {
            if (a_TT != null)
            {
                Transform = a_TT;
            }
        }

        MeshElement3D ModelNode.GetModel()
        {
            return this;
        }
    }

    


    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        //private ObservableCollection<ModelPart> modelPartCollection = new ObservableCollection<ModelPart>();
        //private ModelPart m_last;

        Tool3DParts m_part = new Tool3DParts();

        public MainWindow()
        {
            InitializeComponent();
            DGV.ItemsSource = m_part.GetCollection();
            HelixView.Children.Add(m_part.GetRoot().Node.GetModel());

            //MeshBuilder mb = new MeshBuilder();
            //mb.AddBox(new Point3D(0, 0, 0), 2, 2, 2);
            //mb.AddCone(new Point3D(0, 0, 0), new Point3D(0, 0, 20), 10, true, 50);
            //mb.AddCylinder(new Point3D(0, 0, 0), new Point3D(0, 0, 20), 10, 50);
            //mb.AddTriangle(new Point3D(0, 0, 0), new Point3D(0, 0, 10), new Point3D(10, 0, 10));
            //mb.AddQuad(new Point3D(0, 0, 0), new Point3D(5, 0, 5), new Point3D(5, 0, 0), new Point3D(0, 0, 5));
            //mb.AddSphere(new Point3D(0, 0, 0), 1);
            //mb.AddCubeFace(new Point3D(0, 0, 0), new Vector3D(0, 0, 1), new Vector3D(1, 0, 0), 12, 10, 10);
            

            //obj = new GeometryModel3D { Geometry = mb.ToMesh(), Material = Materials.White };

            

            //mv = new ModelVisual3D { Content = obj, Transform = new };

            //mv = new QuadVisual3D() { Point1 = new Point3D(0, 0, 0), Point2 = new Point3D(5, 0, 0), Point3 = new Point3D(5, 0, 5), Point4 = new Point3D(0, 0, 5) };
            //mv = new MyVisual() { TopRadius = 1, BaseRadius = 2, Height = 1, Transform = tt, Normal = new Vector3D(1, 0, 0)};

            //mv.Children.Add(new RectangleVisual3D() { Width = 10, Length = 10, Transform = tt2 });

            

            //view1.Children.Add(mv);
            //view1.Children.Add(new PipeVisual3D() { Point1 = new Point3D(0, 0, 0), Point2 = new Point3D(0, 0, 10), Diameter = 5});
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            //ModelPart newModel = new ModelPart() { Name = "123", Node = new TruncatedConeNode() { TopRadius = 1, BaseRadius = 2, Height = 1, Normal = new Vector3D(0, 1, 0)} };

            //if (modelPartCollection.Count > 0)
            //{
            //    ModelPart last = modelPartCollection.Last();
            //    last.AddChild(newModel);
            //}
            //else
            //{
            //    HelixView.Children.Add(newModel.Node.GetModel());
            //}

            //modelPartCollection.Add(newModel);
            m_part.AddTruncatedCone();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            var loader = new ModelImporter();
            Model3D currentModel = loader.Load("T6_S1.stl", Dispatcher.CurrentDispatcher);
            HelixView.Children.Add(new ModelVisual3D { Content = currentModel });
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            //ModelPart needDel = DGV.SelectedItem as ModelPart;
            //ModelVisual3D modelDel = needDel.Node.GetModel();

            //var parent = VisualTreeHelper.GetParent(modelDel);
            //var pip = parent.GetType().GetProperty("Children", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            //Visual3DCollection parentChildren = pip.GetValue(parent, null) as Visual3DCollection;
            //parentChildren.Remove(modelDel);

            //if (modelDel.Children.Count != 0)
            //{
            //    Visual3D child = modelDel.Children[0];
            //    modelDel.Children.Remove(child);

            //    int index = modelPartCollection.IndexOf(needDel);
            //    if (index > 0)
            //    {
            //        ModelPart parentModelPart = modelPartCollection.ElementAt(index - 1);
            //        parentModelPart.AddChild(modelPartCollection.ElementAt(index + 1));
            //    }
            //    else
            //    {
            //        parentChildren.Add(child);
            //        child.Transform = new TranslateTransform3D();
            //    }
            //}

            //modelPartCollection.Remove(needDel);
            m_part.DeletePart(DGV.SelectedIndex);
        }

        private void DGV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (m_last != null)
            //{
            //    m_last.Node.GetModel().Material = Materials.Blue;
            //}

            //ModelPart mp = DGV.SelectedItem as ModelPart;
            //if (mp != null)
            //{
            //    mp.Node.GetModel().Material = Materials.Yellow;
            //}
            //m_last = mp;

            m_part.SelectPart(DGV.SelectedIndex);
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    obj.Material = Materials.Red;

        //    //var loader = new ModelImporter();
        //    //Model3D currentModel = loader.Load("CAV.stl", Dispatcher.CurrentDispatcher);
        //    //view1.Children.Add(new ModelVisual3D { Content = currentModel});

        //    tt.OffsetZ += -4;
        //}

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    MyVisual s = mv as MyVisual;
        //    s.TopRadius = 10;
        //    s.Material = Materials.Yellow;

        //    tt2.OffsetZ += -4;

            
        //}
    }
}
