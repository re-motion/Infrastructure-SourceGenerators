// This file is part of the re-motion Framework (www.re-motion.org)
// Copyright (c) rubicon IT GmbH, www.rubicon.eu
// 
// re-motion Framework is free software; you can redistribute it
// and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation; either version 2.1 of the
// License, or (at your option) any later version.
// 
// re-motion is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with re-motion; if not, see http://www.gnu.org/licenses.

using System.Collections.Generic;
using System.Text;

namespace Infrastructure.SourceGenerators.InternalsVisibleTo;

public static class HexFormatter
{
  private static readonly char[] s_hexChars = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'};

  public static string AsHexString (IEnumerable<byte> data)
  {
    var sb = new StringBuilder();
    foreach (var b in data)
    {
      sb.Append(s_hexChars[b >> 4]);
      sb.Append(s_hexChars[b & 0xF]);
    }

    return sb.ToString();
  }
}