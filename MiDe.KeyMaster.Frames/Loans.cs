using MiDe.KeyMaster.App;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiDe.KeyMaster.Frames
{
    public partial class Loans : Form
    {
        private BorrowController controller;

        DataTable? dt = new DataTable();
        BindingSource bi = new BindingSource();
        int pageIndex = 0;
        int PageSize = 20;


        public Loans(BorrowController controller)
        {
            this.controller = controller;
            InitializeComponent();
        }

        private void Loans_Load(object sender, EventArgs e)
        {
            LoadGridView();
        }

        private void LoadGridView()
        {
            dt = controller.GetLoansByPage(0, PageSize);
            bi.DataSource = dt;
            dataGridViewLoans.DataSource = bi;
            dataGridViewLoans.ClearSelection();
        }

        private int GetDisplayedRowsCount()
        {
            int count = dataGridViewLoans.Rows[dataGridViewLoans.FirstDisplayedScrollingRowIndex].Height;
            count = dataGridViewLoans.Height / count;
            return count;
        }

        private void dataGridViewLoans_Scroll(object sender, ScrollEventArgs e)
        {
            int display = dataGridViewLoans.Rows.Count - dataGridViewLoans.DisplayedRowCount(false);
            if (e.Type == ScrollEventType.SmallIncrement || e.Type == ScrollEventType.LargeIncrement)
            {
                if (e.NewValue >= dataGridViewLoans.Rows.Count - GetDisplayedRowsCount())
                {
                    dt = controller.GetLoansByPage(0, PageSize);
                    if (dt != null)
                    {
                        bi.DataSource = dt;
                        dataGridViewLoans.ClearSelection();
                        dataGridViewLoans.FirstDisplayedScrollingRowIndex = display;
                        pageIndex++;
                    }
                }
            }

        }

        private void dataGridViewLoans_SelectionChanged(object sender, EventArgs e)
        {
            dataGridViewLoans.ClearSelection();
        }
    }
}
