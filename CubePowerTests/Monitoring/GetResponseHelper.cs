/* ------------------------------------------------------------------------- */
///
/// Monitoring/GetResponseHelper.cs
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
using System.Reflection;

namespace CubePowerTests.Monitoring
{
    /* --------------------------------------------------------------------- */
    ///
    /// GetResponseHelper
    /// 
    /// <summary>
    /// ローカルに保存したファイルを用いての各種 Client クラスをテストする
    /// 場合、protected メソッドを実行する必要があります。このクラスは、
    /// 該当メソッドを実行するためのヘルパークラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class GetResponseHelper
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Run
        /// 
        /// <summary>
        /// 引数に指定された Client オブジェクトの GetResponse メソッドを
        /// 実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static CubePower.Monitoring.Response Run(CubePower.Monitoring.Client client, string filename, DateTime time)
        {
            var cur  = System.Environment.CurrentDirectory;
            var dir  = System.IO.Path.Combine(cur, "Examples");
            var path = System.IO.Path.Combine(dir, filename);

            using (var stream = new System.IO.FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var type = client.GetType();
                var method = type.GetMethod("GetResponse", BindingFlags.NonPublic | BindingFlags.Instance);
                return method.Invoke(client, new object[] { stream, time }) as CubePower.Monitoring.Response;
            }
        }
    }
}
