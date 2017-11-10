# VirtoCommerce.Quote
VirtoCommerce.Quote module represents Quotes management system.
Key features:
* unlimited number of tiers
* discounts per tier or for the whole quote
* full line items' management even after the initial quote was created
* attachments, dynamic properties support
* regular Order is produced once the Quote is confirmed by a customer.

![Quotes management UI](https://cloud.githubusercontent.com/assets/5801549/15647553/f67473a6-266c-11e6-959d-bc562825a687.png)

# Documentation
User guide:

Developer guide:

# Installation
Installing the module:
* Automatically: in VC Manager go to Configuration -> Modules -> Quote module -> Install
* Manually: download module zip package from https://github.com/VirtoCommerce/vc-module-quote/releases. In VC Manager go to Configuration -> Modules -> Advanced -> upload module package -> Install.

# Settings
* **Quote.EnableQuotes** - flag indicating that quotes are admissible in a particular store;
* **Quote.Status** - statuses that a Quote can be in (New, Processing, etc.);
* **Quote.QuoteRequestNewNumberTemplate** - template for new Quote number generation.


# Available resources
* Module related service implementations as a <a href="https://www.nuget.org/packages/VirtoCommerce.QuoteModule.Data" target="_blank">NuGet package</a>
* API client as a <a href="https://www.nuget.org/packages/VirtoCommerce.QuoteModule.Client" target="_blank">NuGet package</a>
* API client documentation http://demo.virtocommerce.com/admin/docs/ui/index#!/Quote_module

# License
Copyright (c) Virto Solutions LTD.  All rights reserved.

Licensed under the Virto Commerce Open Software License (the "License"); you
may not use this file except in compliance with the License. You may
obtain a copy of the License at

http://virtocommerce.com/opensourcelicense

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
implied.
