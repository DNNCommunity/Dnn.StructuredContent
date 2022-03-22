<%@ Control Language="C#" Inherits="StructuredContent.Visualizer" AutoEventWireup="true" Explicit="true" CodeBehind="Visualizer.ascx.cs" %>

<div ng-app="StructuredContent" ng-cloak>
    <div visualizer></div>
</div>

<script>
  var ModuleId = <%= ModuleId %>;
  var siteRoot = "<%= this.ResolveUrl("~/") %>";
</script>

