﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using XLant;

namespace XlantWord
{
    public partial class ClientForm : Form
    {
        public XLMain.Client selectedClient;

        public ClientForm()
        {
            InitializeComponent();
            this.CenterToParent();
        }

        public class EntityCouplet
        {
            public string crmID { get; set; }
            public string name { get; set; }
        }

        private void Search()
        {
            //clear existing entries if any
            ClientListBox.Items.Clear();

            //start a new search
            string searchStr = SearchTB.Text;
            Boolean IncLost = IncLostCheck.Checked;
            ClientListBox.DisplayMember = "Name";
            ClientListBox.ValueMember = "crmid";
            SqlDataReader xlReader = null;
            
            string query = "Select clientcode + ' - ' + name as Name, crmID from Client where ((ClientCode like '%"+ searchStr + "%') Or (Name Like  '%" + searchStr + "%'))";
            
            if (!IncLost)
            {
                query += " and status in ('New', 'Active')";
            }
            query += " order by name";

            xlReader = XLSQL.ReaderQuery(query);

            if (xlReader != null)
            {
                if (xlReader.HasRows)
                {
                    while (xlReader.Read())
                    {
                        EntityCouplet newEntity = new EntityCouplet();
                        newEntity.crmID = xlReader.NiceString("CRMId");
                        newEntity.name = xlReader.NiceString("Name");
                        ClientListBox.Items.Add(newEntity);
                    }
                }
                else
                {
                    MessageBox.Show("No Records found");
                }
            }
            else
            {
                MessageBox.Show("Unable to connect to the database.");
            }
            
        }

        private void SearchBtn_Click_1(object sender, EventArgs e)
        {
            Search();
        }

        private void ClientListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EntityCouplet selectedItem = (EntityCouplet)ClientListBox.SelectedItem;
            selectedClient = XLMain.Client.FetchClient(selectedItem.crmID);
            this.Close();
        }

        private void SearchTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                Search();
            }
        }
    }
}

