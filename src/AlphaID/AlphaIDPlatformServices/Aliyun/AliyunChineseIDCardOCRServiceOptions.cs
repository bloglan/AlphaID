﻿namespace AlphaIDPlatformServices.Aliyun;

/// <summary>
/// 阿里云身份证OCR识别服务选项。
/// </summary>
public class AliyunChineseIDCardOCRServiceOptions
{
    /// <summary>
    /// 服务基地址。
    /// </summary>
    public string ServiceBaseUrl { get; set; } = "https://cardnumber.market.alicloudapi.com/rest/160601/ocr/ocr_idcard.json";

    /// <summary>
    /// APPCODE。
    /// </summary>
    public string AppCode { get; set; } = "ab77a94f915949bba85304cacac82f47";
}