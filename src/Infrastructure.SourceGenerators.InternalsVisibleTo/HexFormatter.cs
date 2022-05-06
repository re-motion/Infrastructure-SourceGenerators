// SPDX-FileCopyrightText: Copyright (c) rubicon IT GmbH, www.rubicon.eu
// SPDX-License-Identifier: LGPL-2.1-or-later

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