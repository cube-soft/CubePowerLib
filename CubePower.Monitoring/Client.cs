/* ------------------------------------------------------------------------- */
///
/// Client.cs
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
    /// Client
    /// 
    /// <summary>
    /// サーバへ電力情報を問い合わせるためのクライアントの基底クラスです。
    /// </summary>
    /// 
    /// <remarks>
    /// このクラスを直接インスタンス化する事はできません。必要な実装を行った
    /// 各サブクラスをインスタンス化して下さい。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class Client
    {
        #region Initialization and Termination

        /* ----------------------------------------------------------------- */
        ///
        /// Client (constructor)
        /// 
        /// <summary>
        /// 引数に指定された地域を利用して、オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected Client(Area area)
        {
            _area = area;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Area
        /// 
        /// <summary>
        /// オブジェクトが電力情報の取得対象としている地域を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Area Area
        {
            get { return _area; }
        }

        #endregion

        #region Public methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetResponse
        /// 
        /// <summary>
        /// 引数に指定された日時に対応する電力情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Response GetResponse(DateTime time)
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) return null;

            var request = System.Net.WebRequest.Create(GetUrl(time));
            request.Proxy = null;

            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                return GetResponse(stream, time);
            }
        }

        #endregion

        #region Need to implement in inherited classes

        /* ----------------------------------------------------------------- */
        ///
        /// GetUrl
        /// 
        /// <summary>
        /// 電力情報を取得するための URL を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual string GetUrl(DateTime time)
        {
            throw new NotImplementedException();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetResponse
        /// 
        /// <summary>
        /// 引数に指定されたストリームから問い合わせのあった日時に対応する
        /// レスポンスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual Response GetResponse(System.IO.Stream stream, DateTime time)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Variables
        private Area _area;
        #endregion
    }
}
