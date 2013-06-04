/* ------------------------------------------------------------------------- */
///
/// Response.cs
///
/// Copyright (c) 2013 CubeSoft, Inc. All rights reserved.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU General Public License as published by
/// the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU General Public License for more details.
///
/// You should have received a copy of the GNU General Public License
/// along with this program.  If not, see < http://www.gnu.org/licenses/ >.
///
/* ------------------------------------------------------------------------- */
using System;

namespace CubePower.Monitoring
{
    /* --------------------------------------------------------------------- */
    ///
    /// Response
    ///
    /// <summary>
    /// サーバ等から取得した電力情報に関するレスポンスを格納するための
    /// クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Response
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Area
        /// 
        /// <summary>
        /// 対象となる電力会社の地域を取得、または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Area Area
        {
            get { return _area; }
            set { _area = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Time
        ///
        /// <summary>
        /// 取得した情報の取得時刻を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Usage
        ///
        /// <summary>
        /// 現在の電力消費量を取得、または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Usage
        {
            get { return _usage; }
            set { _usage = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UsageRatio
        ///
        /// <summary>
        /// 現在の電力消費割合をパーセンテージ単位で取得します。小数点以下は
        /// 四捨五入されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int UsageRatio
        {
            get
            {
                if (_capacity == 0) return 0;
                double ratio = _usage / (double)_capacity;
                ratio *= 100;
                return (int)(ratio + 0.5);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Capacity
        ///
        /// <summary>
        /// 該当日の電力最大供給量を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Capacity
        {
            get { return _capacity; }
            set { _capacity = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Unit
        /// 
        /// <summary>
        /// Usage, および Capacity で取得できる値の単位を表す文字列を取得、
        /// または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        #endregion

        #region Variables
        private Area _area;
        private DateTime _time;
        private int _usage;
        private int _capacity;
        private string _unit;
        #endregion
    }
}
