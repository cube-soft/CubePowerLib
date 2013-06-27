/* ------------------------------------------------------------------------- */
///
/// Area.cs
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
    /// Area
    ///
    /// <summary>
    /// 電力情報を取得可能な地域一覧を定義した列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum Area
    {
        Hokkaido,   // 北海道電力
        Tohoku,     // 東北電力
        Hokuriku,   // 北陸電力
        Tokyo,      // 東京電力
        Chubu,      // 中部電力
        Kansai,     // 関西電力
        Chugoku,    // 中国電力
        Shikoku,    // 四国電力
        Kyushu,     // 九州電力
    }
}
