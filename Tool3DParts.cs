using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Threading;
using HelixToolkit.Wpf;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Collections.ObjectModel;

namespace TestHelix3D
{
    public interface ModelNode
    {
        Transform3D GetTT();
        void SetTT(Transform3D a_TT);
        MeshElement3D GetModel();
    }

    public class ModelPart
    {
        public string Name
        {
            get;
            set;
        }

        public ModelNode Node
        {
            get;
            set;
        }

        public void AddChild(ModelPart a_model)
        {
            if (a_model != null)
            {
                a_model.Node.SetTT(Node.GetTT());
                Node.GetModel().Children.Add(a_model.Node.GetModel());
            }
        }
    }

    public class Tool3DParts
    {
        private class RootNode : MeshElement3D, ModelNode
        {
            private TranslateTransform3D ChildNode = new TranslateTransform3D();

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

            protected override MeshGeometry3D Tessellate()
            {
                var builder = new MeshBuilder(false, true);
                return builder.ToMesh();
            }
        }

        private ModelPart m_root = new ModelPart() { Name = "Root", Node = new RootNode() };
        private ObservableCollection<ModelPart> m_collection = new ObservableCollection<ModelPart>();
        private ModelPart m_lastSelect;
        private uint m_counter = 0;

        public Tool3DParts()
        {

        }

        public ObservableCollection<ModelPart> GetCollection()
        {
            return m_collection;
        }

        public ModelPart GetRoot()
        {
            return m_root;
        }

        public void AddTruncatedCone()
        {
            ModelPart newModel = new ModelPart() { Name = "Part" + m_counter++, Node = new TruncatedConeNode() { TopRadius = 1, BaseRadius = 2, Height = 1, Normal = new Vector3D(0, 1, 0), Fill = new SolidColorBrush(Colors.Blue.ChangeAlpha(0x20)) } };

            if (m_collection.Count > 0)
            {
                ModelPart last = m_collection.Last();
                last.AddChild(newModel);
            }
            else
            {
                m_root.Node.GetModel().Children.Add(newModel.Node.GetModel());
            }
            
            m_collection.Add(newModel);
        }

        public void DeletePart(int a_index)
        {
            if (a_index < m_collection.Count && a_index >= 0)
            {
                ModelPart needDel = m_collection[a_index];
                MeshElement3D modelDel = needDel.Node.GetModel();

                var parent = VisualTreeHelper.GetParent(modelDel);
                var pip = parent.GetType().GetProperty("Children", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                Visual3DCollection parentChildren = pip.GetValue(parent, null) as Visual3DCollection;
                parentChildren.Remove(modelDel);

                if (modelDel.Children.Count != 0)
                {
                    modelDel.Children.Clear();
                    
                    int index = m_collection.IndexOf(needDel);
                    if (index > 0)
                    {
                        ModelPart parentModelPart = m_collection.ElementAt(index - 1);
                        parentModelPart.AddChild(m_collection.ElementAt(index + 1));
                    }
                    else
                    {
                        m_root.AddChild(m_collection.ElementAt(index + 1));
                    }
                }

                m_collection.Remove(needDel);
            }
        }

        public void SelectPart(int a_index)
        {
            if (a_index < m_collection.Count && a_index >= 0)
            {
                if (m_lastSelect != null)
                {
                    //m_lastSelect.Node.GetModel().Material = Materials.Blue;
                    m_lastSelect.Node.GetModel().Fill = new SolidColorBrush(Colors.Blue.ChangeAlpha(0x20));
                }

                ModelPart mp = m_collection[a_index];
                if (mp != null)
                {
                    //mp.Node.GetModel().Material = Materials.Yellow;
                    mp.Node.GetModel().Fill = new SolidColorBrush(Colors.Yellow.ChangeAlpha(0x20));
                }
                m_lastSelect = mp;
            }
        }
    }
}
