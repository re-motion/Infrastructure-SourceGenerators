// SPDX-FileCopyrightText: Copyright (c) rubicon IT GmbH, www.rubicon.eu
// SPDX-License-Identifier: LGPL-2.1-or-later

using System;

namespace Infrastructure.SourceGenerators.InternalsVisibleTo.Tests
{
  public static class WellKnownPublicKey
  {
    private const string c_publicKeyBase64 =
        "ACQAAASAAACUAAAABgIAAAAkAABSU0ExAAQAAAEAAQAV9fXiFuyUjCLP/wGbj0jpbSFicdtRxBnsgIyreYfTV2EWCDN9R1lkooimlmuB0WltxlaMLnHE8+4Ropga9I1hcVE7tOMrkpVliqPfU6eO1+WKPdXKBR4sFRUsz1rrCFd/U25o8fQFYzAbYNIycb4I7akGY/hyFbr6aRAqQA980g==";

    public static byte[] Data => Convert.FromBase64String(c_publicKeyBase64);

    public static string FormattedString => HexFormatter.AsHexString(Data);
  }
}