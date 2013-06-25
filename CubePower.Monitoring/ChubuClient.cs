/* ------------------------------------------------------------------------- */
///
/// ChubuClient.cs
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
using System.Diagnostics;

namespace CubePower.Monitoring
{
    /* --------------------------------------------------------------------- */
    ///
    /// ChubuClient
    /// 
    /// <summary>
    /// 中部電力から電力情報を取得するためのクラスです。
    /// </summary>
    /// 
    /// <seealso cref="http://denki-yoho.chuden.jp/"/>
    ///
    /* --------------------------------------------------------------------- */
    public class ChubuClient : Client
    {
        #region Initialization and Termination

        /* ----------------------------------------------------------------- */
        ///
        /// ChubuClient (constructor)
        /// 
        /// <summary>
        /// 既定の値でオブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ChubuClient() : base(Area.Chubu) { }

        #endregion

        #region Override methods

        private bool today = true;

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
            if (time >= DateTime.Today) return "http://denki-yoho.chuden.jp/denki_yoho_content_data/juyo_cepco003.csv";

            today = false;
            if (time >= new DateTime(2013, 1, 1)) return "http://denki-yoho.chuden.jp/denki_yoho_content_data/juyo_current_term.csv";

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
                response.Capacity = 0;

                if (today)
                {
                    // 該当日の電力最大供給量(3行目)
                    for (int line = 1; line <= 2; ++line) sr.ReadLine();
                    if (!GetCapacity(sr, response)) return null;

                    // 現在の電力消費量、取得した情報の取得時刻(77行目以降)
                    for (int line = 4; line <= 76; ++line) sr.ReadLine();
                    if (!GetUsage(sr, response)) return null;
                    return response;
                }
                else // 当日のデータ以外は年度単位にデータがまとめられているので、別の方法で取得
                {
                    sr.ReadLine();
                    return CsvUnityCase(sr, response) ? response : null;
                }
            }
        }

        #endregion
    }
}
