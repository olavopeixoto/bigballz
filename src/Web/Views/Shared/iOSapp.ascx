<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<link rel="apple-touch-icon" href="<%=Url.VersionedContent("~/public/images/grana.png") %>">
    
<!-- Allow web app to be run in full-screen mode. -->
<meta name="apple-mobile-web-app-capable" content="yes">

<!-- Make the app title different than the page title. -->
<meta name="apple-mobile-web-app-title" content="BigBallz">
    
<!-- Configure the status bar. -->
<meta name="apple-mobile-web-app-status-bar-style" content="default">
    
    
    
<!-- Disable automatic phone number detection. -->
<meta name="format-detection" content="telephone=no">
    
<!-- Icons -->
<link rel="apple-touch-icon" href="<%=Url.VersionedContent("~/public/images/152.png") %>">
<link rel="apple-touch-icon" sizes="76x76" href="<%=Url.VersionedContent("~/public/images/76.png") %>">
<link rel="apple-touch-icon" sizes="120x120" href="<%=Url.VersionedContent("~/public/images/120.png") %>">
<link rel="apple-touch-icon" sizes="152x152" href="<%=Url.VersionedContent("~/public/images/152.png") %>">
    
<!-- iPhone X startup image -->
<link rel="apple-touch-startup-image" href="<%=Url.VersionedContent("~/public/images/splash-x.png") %>" />

<meta property="fb:app_id" content="<%=ConfigurationManager.AppSettings["fb-appid"]%>"/>