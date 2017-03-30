# PrismRibbonRegionAdapter

A RegionAdapter for the Ribbon native WPF control for the PRISM composite WPF application block.

> **Forked from:** https://prismribbonregionadapter.codeplex.com/

## This adapter supports the following:

- Merge RibbonTabs and ContextualTabGroups
  - also merge RibbonGroups
 - Merge QuickAccessToolBar
 -   Merge ApplicationMenu
 -   handle sorting/ordering via attached property
 -   detect identical tabls/ribbongroups via their Header property or an attached MergeKey property (the attached-property has priority)
 -   detaches the merged elements from their old parent into the main ribbon, preserving the DataContext of each element

New with Version 0.20:

- removing/unmerging

New with Version 0.11:

- Merge any control which derives from ItemsControl, such as ContextMenu, ListBox, ListView, etc.
  - Register the MergingItemsControlRegionAdapter for these controls
