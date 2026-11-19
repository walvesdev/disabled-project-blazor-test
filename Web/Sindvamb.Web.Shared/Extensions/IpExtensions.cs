// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License
// See the LICENSE file in the project root for more information.
// Maintainer: Argo Zhang(argo@live.ca) Website: https://www.blazor.zone

namespace Sindvamb.Web.Shared.Extensions;

public static class IpExtensions
{
    public static string MaskIpString(this string ip)
    {
        // var ret = ip;
        // if (!string.IsNullOrEmpty(ip))
        // {
        //     var segments = ip.Split('.');
        //     segments[^1] = "###";
        //     ret = string.Join('.', segments);
        // }
        // return ret;
        return ip;
    }

    public static string ToIPv4String(this IPAddress? address)
    {
        string str = (address ?? IPAddress.IPv6Loopback).ToString();
        return !str.StartsWith("::ffff:") ? str : (address ?? IPAddress.IPv6Loopback).MapToIPv4().ToString();
    }
}
