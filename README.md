# VirtoCommerce.Quote
VirtoCommerce.Quote module represents Quotes management system.

**Key features:**
* Unlimited number of tiers
* Discounts per tier or for the whole quote
* Full line items' management even after the initial quote was created
* Attachments, dynamic properties support
* Regular Order is produced once the Quote is confirmed by a customer.

# Documentation
**Request for Quote (RFQ)**

Quotation extension for VirtoCommerce helps store owners to easily manage quotes from clients and send a precise cost estimate without the use of handmade proposals. It automates the process by introducing RFQ form that allows easy input of data by the customers about any product or service. Customers can also upload images and documents to explain their quotes better and receive email notifications about their proposals. It facilitates fast track and hassle free ask for quote process without the need for an external form.

**Key Features - Request for Quote:**
* Enable and disable RFQ form from site
* Automated RFQ Process with simple form
* Easy to find client requests from manager
* Automatics validation of form fields
* Email alerts to admin/site owners on RFQ
* Email notifications to clients
* Edit form fields
* Customers/Managers can upload multiple attachments

**Merchant Benefits**

Automated Process:

It enhances the Request for Quote process from the traditional process in which the customers has to make a proposal by hand and send it to the site owner by taking pictures of it or scanning the pages. It gives an online RFQ form which the customers have to fill and send it directly to the admin. You can also set predefined emails to further automate the process.

RFQ Manager:

It sends admin email notifications each time a request for quote is made. The admin can find any quote from the RFQ manager with name and details of the client. You can update the status of the quote from here and send the estimate price of the product or service.

**Customer Benefits**

RFQ form:

Customers can now easily enter details in the required fields and send a request for quote instead of the traditional method where the proposal had to be made by hand. It also allows customers to upload files and elaborate the quote further while filling the form.

Email Alerts:

It facilitates easy and fast track process of acquiring cost estimate without the need for any personal messages to the site owner. Email notifications inform the customer when the proposal has been received or updated by the admin. When the quote status is changed by the admin, the customers will be notified as well.

**Main workflow overview**

![](https://user-images.githubusercontent.com/23373328/36895674-6f160f64-1e18-11e8-98a9-7afc9c463af4.png)
  
A buyer in VirtoCommerce store can create a Request for Quote (RFQ) for unique variations of goods and services that are offered in the catalog. A buyer can create an RFQ as simple as adding items to regular shopping cart.

A requisition list is used by buyers to add products to RFQs. Buyers can include multiple products in one RFQ and define unique specifications for each product. They can include attachments on the RFQ or product specification level. They can also specify the terms and conditions for the transaction. When the buyer submits an RFQ request, it is placed into a "Processing" state. A seller can view the RFQ in VirtoCommerce manager and submit a response when the request is in an "active" state. The buyer can also change or cancel the RFQ.

![](https://user-images.githubusercontent.com/23373328/36860749-d71aecb2-1d8a-11e8-9f17-5ffdea424c51.png)

For an existing RFQ, a buyer can negotiate the price at the category level. The RFQ summary and list pages within the VirtoCommerce manager displays this information.

When sellers respond to an RFQ, they have the option of responding to each attachment, terms and conditions, product, category, as well as to each product specification or comment. Sellers and buyers negotiate aspects of RFQs (eg., price adjustments at the percentage or fixed price levels are a common point of negotiation). Sellers have the option of specifying a fulfillment center or substituting a product, if the buyer has provided that option in the request. A seller can also modify or cancel the RFQ.

![](https://user-images.githubusercontent.com/23373328/36860818-0361194a-1d8b-11e8-8626-72e1930d92f6.png)

Once sellers have responded to the RFQs, the buyer opens the RFQ and evaluates the responses to choose a winner (or multiple winners). When the RFQ response is accepted by the buyer and the seller is notified, the RFQ transaction is completed using one of the following processes:
* The buyer places an order that already contains the RFQ information.
* A contract already containing the RFQ information is created.
* The RFQ goes to the next round.

A record of the RFQ is maintained in the RFQ request list for a predetermined period, so that you can copy an RFQ that you repeatedly use. Responses are retained for the same period to facilitate a seller's response to similar requests from the same buyer.

**RFQ states**

![](https://user-images.githubusercontent.com/23373328/36964972-e15bbcaa-2060-11e8-9623-69b6bd967ec4.png)

![](https://user-images.githubusercontent.com/23373328/36972563-402c6704-2078-11e8-91a4-43d3cdd9b148.png)

**Enable quotes for store**

To enable quotes functionality in your store, switch EnableQuotes on in Commerce manager Settings of selected Store.

![](https://user-images.githubusercontent.com/23373328/36892520-a68c15fc-1e0d-11e8-93c9-a0b4aca69e94.png)

**How to use Quotes in the example of VirtoCommerce Storefront**

1. Add product to quote.

![](https://user-images.githubusercontent.com/23373328/36863170-9e8817ec-1d91-11e8-8f37-4f0da63145e7.png)

![](https://user-images.githubusercontent.com/23373328/36868793-1fc634c8-1da2-11e8-819f-c117b20afa43.png)

2. Create and submit RFQ.

![](https://user-images.githubusercontent.com/23373328/36898195-76a10ee6-1e22-11e8-824f-8b5cb2854b8e.png)

3. Accept/reject proposed offers.

![](https://user-images.githubusercontent.com/23373328/36897976-7e3e6d02-1e21-11e8-92b1-c9ceb44690ab.png)

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
