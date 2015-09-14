using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PriceUploader
{
    public partial class FormCategories : Form
    {
        public string CategoryValue
        {
            get { return treeViewCategories.SelectedNode.Name; }
        }
        public FormCategories()
        {
            InitializeComponent();
        }

        public System.Windows.Forms.TreeView TreeViewCategories
        {
            get { return treeViewCategories; }
        }


        internal void Init(DataTable TableProductCategory)
        {
            List<Category>categories = new List<Category>();                  
            foreach (var item in TableProductCategory.AsEnumerable())
            {
                categories.Add(new Category
                {
                    pc_id = Convert.ToInt32(item.ItemArray[0]),
                    pc_parent_id = Convert.ToInt32(item.ItemArray[1]),
                    pc_name = Convert.ToString(item.ItemArray[2]),
                    pc_description = Convert.ToString(item.ItemArray[8])
                });
            }

            treeViewCategories.Nodes.Clear();

            var RootCategories = categories.Where(x => x.pc_parent_id == 0).ToList();

            foreach(Category category in RootCategories)
            {
                // Root Nodes
                TreeNode node = null; 
                //node = treeViewCategories.Nodes.Add(category.pc_id.ToString(), category.pc_name);
                draw(categories, category, node);

            }
        }

        private void draw(List<Category> categories, Category category, TreeNode node)
        {
            var subCategories = categories.Where(x => x.pc_parent_id == category.pc_id).ToList();
            
            if (subCategories.Count == 0)
            {
                drawLeaf(category, node);
            }
            else
            {
                drawNonLeaf(categories, category, node);
            }
        }

        private void drawNonLeaf(List<Category> categories, Category category, TreeNode node)
        {
            var subCategories = categories.Where(x => x.pc_parent_id == category.pc_id).ToList();
            //Add Node 
            if (node == null)
                node = treeViewCategories.Nodes.Add(category.pc_id.ToString(), category.pc_name);
            else
                node = node.Nodes.Add(category.pc_id.ToString(), category.pc_name);
            foreach (Category cat in subCategories)
            {
                draw(categories, cat, node);
            }

        }

        private void drawLeaf(Category category, TreeNode node)
        {
            if (node == null)
                node = treeViewCategories.Nodes.Add(category.pc_id.ToString(), category.pc_name);
            else
                node.Nodes.Add(category.pc_id.ToString(), category.pc_name);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }

    public class Category
    {
        public int pc_id = 0;
        public int pc_parent_id;       
        public string pc_name;
        public string pc_description;
        public int pc_id_from_category_charge;
    }
}
