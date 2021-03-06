﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiBot.Repository.Base;

namespace HiBot.Repository.Infrastructure
{
   public class Paginations : IPagination
    {
        #region Member Variables
        protected int _pageNumber;
        protected int _totalItems;
        protected int _pageSize;
        protected bool _hasNextPage;
        protected bool _hasPreviousPage;
        protected int _lastItem;
        protected int _firstItem;
        #endregion
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }
        #region Public Properties

        public bool HasNextPage
        {
            get => _hasNextPage; 
            set => _hasNextPage = value; 
        }
        public bool HasPreviousPage
        {
            get => _hasPreviousPage; 
            set => _hasPreviousPage = value; 
        }
        public int LastItem
        {
            get { return _lastItem; }
            set { _lastItem = value; }
        }
        public int FirstItem
        {
            get { return _firstItem; }
            set { _firstItem = value; }
        }
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }
        public int TotalItems
        {
            get { return _totalItems; }
            set { _totalItems = value; }
        }
        public int TotalPages
        {
            get => (int)Math.Ceiling(((double)TotalItems) / PageSize); 
        }
        public int PageNumber
        {
            get { return _pageNumber; }
            set { _pageNumber = value; }
        }
        #endregion

    }
}
