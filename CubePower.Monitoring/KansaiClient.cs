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
            return "http://www.kepco.co.jp/yamasou/juyo1_kansai.csv";
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
            if (stream == null)
            {
                throw new NullReferenceException();
            }

            using (var sr = new StreamReader(stream))
            {
                var response = new Response();

                // 電力会社の地域
                response.Area = this.Area;

                // Usage, および Capacity で取得できる値の単位を表す文字列
                response.Unit = "万kW";

                // 該当日の電力最大供給量(3行目)
                sr.ReadLine(); sr.ReadLine();
                string[] fields = sr.ReadLine().Split(',');
                response.Capacity = int.Parse(fields[0]);

                // 現在の電力消費量、取得した情報の取得時刻(50行目以降)
                for (int i = 0; i < 46; i++) { sr.ReadLine(); } // 49行目までスキップ
                response.Time = time;
                response.Usage = 0;
                var re = new System.Text.RegularExpressions.Regex(@"^[\d]{4}");
                string t = time.ToString("yyyy'/'M'/'d");
                bool flg = false;
                for (string line = sr.ReadLine(); line != null; line = sr.ReadLine())
                {
                    if (re.IsMatch(line))
                    {
                        fields = line.Split(',');
                        try
                        {
                            DateTime d =
                                DateTime.ParseExact(fields[0] + ',' + fields[1],
                                "yyyy'/'M'/'d','H':'mm",
                                System.Globalization.DateTimeFormatInfo.InvariantInfo);
                            if (time >= d) // 指定の日時に最も近い、指定の日時より早い日時
                            {
                                response.Time = d;
                                if (fields.Length > 2) response.Usage = int.Parse(fields[2]);
                                flg = true;
                            }
                        }
                        catch (FormatException) { }  // フォーマットの不一致は無視
                    }
                }

                return flg ? response : null;
            }
        }

        #endregion
    }
}
