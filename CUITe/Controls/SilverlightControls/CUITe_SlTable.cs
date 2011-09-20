﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.SilverlightControls;

namespace CUITe.Controls.SilverlightControls
{
    public enum CUITe_SlTableSearchOptions
    {
        Normal,
        NormalTight,
        Greedy,
        StartsWith,
        EndsWith
    }

    public class CUITe_SlTable : CUITe_ControlBase
    {
        private SilverlightTable _SlTable;

        public CUITe_SlTable(string sSearchParameters) : base(sSearchParameters) { }

        public void Wrap(SilverlightTable control)
        {
            base.Wrap(control);
            this._SlTable = control;
        }

        /// <summary>
        /// Helps you wrap a SilverlightTable control with a CUITe_SlTable to leverage CUITe's convenient methods.
        /// </summary>
        /// <param name="control"></param>
        /// <example>
        /// <code>
        /// CUITe_SlTable tblEdit = new CUITe_SlTable();
        /// tblEdit.WrapReady(edit);
        /// tblEdit.FindCellAndClick(10, 3);
        /// </code>
        /// Here 'edit' is a SilverlightTable object.
        /// </example>
        public void WrapReady(SilverlightTable control)
        {
            base.WrapReady(control);
            this._SlTable = control;
        }

        public SilverlightTable UnWrap()
        {
            return this._SlTable;
        }

        public int RowCount
        {
            get
            {
                this._SlTable.WaitForControlReady();
                return this._SlTable.RowCount;
            }
        }

        public void FindRowAndClick(int iCol, string sValueToSearch)
        {
            int iRow = FindRow(iCol, sValueToSearch, CUITe_SlTableSearchOptions.Normal);
            Mouse.Click(this.GetCell(iRow, iCol));
        }

        public void FindRowAndClick(int iCol, string sValueToSearch, CUITe_SlTableSearchOptions option)
        {
            int iRow = FindRow(iCol, sValueToSearch, option);
            Mouse.Click(this.GetCell(iRow, iCol));
        }

        public void FindRowAndDoubleClick(int iCol, string sValueToSearch)
        {
            int iRow = FindRow(iCol, sValueToSearch, CUITe_SlTableSearchOptions.Normal);
            Mouse.DoubleClick(this.GetCell(iRow, iCol));
        }

        public void FindRowAndDoubleClick(int iCol, string sValueToSearch, CUITe_SlTableSearchOptions option)
        {
            int iRow = FindRow(iCol, sValueToSearch, option);
            Mouse.DoubleClick(this.GetCell(iRow, iCol));
        }

        public void FindCellAndClick(int iRow, int iCol)
        {
            Mouse.Click(this.GetCell(iRow, iCol));
        }

        public void FindCellAndDoubleClick(int iRow, int iCol)
        {
            Mouse.DoubleClick(this.GetCell(iRow, iCol));
        }

        public int FindRow(int iCol, string sValueToSearch, CUITe_SlTableSearchOptions option)
        {
            this._SlTable.WaitForControlReady();
            int iRow = -1;
            int rowCount = -1;
            foreach (SilverlightRow cont in this._SlTable.Rows)
            {
                rowCount++;
                int colCount = -1;
                foreach (SilverlightCell cell in cont.Cells)
                {
                    colCount++;
                    bool bSearchOptionResult = false;
                    if (colCount == iCol)
                    {
                        if (option == CUITe_SlTableSearchOptions.Normal)
                        {
                            bSearchOptionResult = (sValueToSearch == cell.Value);
                        }
                        else if (option == CUITe_SlTableSearchOptions.NormalTight)
                        {
                            bSearchOptionResult = (sValueToSearch == cell.Value.Trim());
                        }
                        else if (option == CUITe_SlTableSearchOptions.StartsWith)
                        {
                            bSearchOptionResult = cell.Value.StartsWith(sValueToSearch);
                        }
                        else if (option == CUITe_SlTableSearchOptions.EndsWith)
                        {
                            bSearchOptionResult = cell.Value.EndsWith(sValueToSearch);
                        }
                        else if (option == CUITe_SlTableSearchOptions.Greedy)
                        {
                            bSearchOptionResult = (cell.Value.IndexOf(sValueToSearch) > -1);
                        }
                        if (bSearchOptionResult == true)
                        {
                            iRow = rowCount;
                            break;
                        }
                    }
                }
                if (iRow > -1) break;
            }
            return iRow;
        }

        public string GetCellValue(int iRow, int iCol)
        {
            string sResult = "";
            SilverlightCell _SlCell = this.GetCell(iRow, iCol);
            if (_SlCell != null) sResult = _SlCell.Value;
            return sResult;
        }

        private SilverlightCell GetCell(int iRow, int iCol)
        {
            this._SlTable.WaitForControlReady();
            SilverlightCell _SlCell = null;
            int rowCount = -1;
            foreach (SilverlightRow cont in this._SlTable.Rows)
            {
                rowCount++;
                if (rowCount == iRow)
                {
                    int colCount = -1;
                    foreach (SilverlightCell cell in cont.Cells)
                    {
                        colCount++;
                        if (colCount == iCol)
                        {
                            _SlCell = cell;
                            break;
                        }
                    }
                }
                if (_SlCell != null)
                {
                    break;
                }
            }
            return _SlCell;
        }

        public CUITe_SlCheckBox GetRowHeaderCheckBox(int iRow)
        {
            SilverlightCheckBox _checkbox = (SilverlightCheckBox)this._SlTable.Rows[iRow].GetChildren()[0].GetChildren()[0];
            CUITe_SlCheckBox retObj = new CUITe_SlCheckBox("*");
            retObj.WrapReady(_checkbox);
            return retObj;
        }
    }
}
