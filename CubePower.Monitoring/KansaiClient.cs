/* ------------------------------------------------------------------------- */
///
/// KansaiClient.cs
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
using System.IO;

namespace CubePower.Monitoring
{
    /* --------------------------------------------------------------------- */
    ///
    /// KansaiClient
    /// 
    /// <summary>
    /// 関西電力から電力情報を取得するためのクラスです。
    /// </summary>
    /// 
    /// <seealso cref="http://www.kepco.co.jp/setsuden/graph/"/>
    ///
    /* --------------------------------------------------------------------- */
    public class KansaiClient : Client
    {
        #region Initialization and Termination

        /* ----------------------------------------------------------------- */
        ///
        /// KansaiClient (constructor)
        /// 
        /// <summary>
        /// 既定の値でオブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public KansaiClient() : base(Area.Kansai) { }

        #endregion

        #region Override methods

        //private bool today = true;

        /* ----------------------------------------------------------------- */
        ///
        /// GetUrl
        /// 
        /// <summary>
        /// 電力情報を取得するための URL を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override string GetUrl(DateTime time)
        {
            if (time >= DateTime.Today) return "http://www.kepco.co.jp/yamasou/juyo1_kansai.csv";

            //today = false;
            // 当日以外のデータの収集方法が未定。関電はZIP形式でcsvを保管しているため。
            //if (time >= new DateTime(2013, 3, 30)) return String.Format("http://www.tepco.co.jp/forecast/html/images/juyo-{0}.csv", time.Year);
            //else if (time >= new DateTime(2012, 12, 1)) return String.Format("http://www.tepco.co.jp/forecast/html/images/juyo-{0}.csv", time.Year);
            //else if (time >= new DateTime(2012, 9,  8)) return String.Format("http://www.tepco.co.jp/forecast/html/images/juyo-{0}.csv", time.Year);
            //else if (time >= new DateTime(2012, 6, 30)) return String.Format("http://www.tepco.co.jp/forecast/html/images/juyo-{0}.csv", time.Year);
            //else if (time >= new DateTime(2012, 3, 31)) return String.Format("http://www.tepco.co.jp/forecast/html/images/juyo-{0}.csv", time.Year);
            //else if (time >= new DateTime(2011, 12, 1)) return String.Format("http://www.tepco.co.jp/forecast/html/images/juyo-{0}.csv", time.Year);
            //else if (new DateTime(2011, 9, 21) <= time && time >= new DateTime(2011, 6, 30)) return String.Format("http://www.tepco.co.jp/forecast/html/images/juyo-{0}.csv", time.Year);
            return null;
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
        protected override Response GetResponse(System.IO.Stream stream, DateTime time)
        {
            if (stream == null) throw new NullReferenceException();

            using (var sr = new StreamReader(stream))
            {
                var response = new Response();
                response.Area = this.Area;
                response.Unit = "万kW";
                response.Time = time;
                response.Usage = 0;

                // 該当日の電力最大供給量(3行目)
                for (int line = 1; line <= 2; ++line) sr.ReadLine();
                if (!GetCapacity(sr, response)) return null;

                // 現在の電力消費量、取得した情報の取得時刻(50行目以降)
                for (int i = 4; i <= 49; i++) sr.ReadLine();
                if (!GetUsage(sr, response)) return null;
                
                return response;
            }
        }

        #endregion
    }
}
