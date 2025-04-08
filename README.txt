AdventureWorks eCommerce  

User Stories:  
   Customer  
Visit website and add products to cart.  
Register account at checkout.  
Add credit card.  
Create Order.  
View order status.  
Track Shipping.  
Receive emails  
   Admin  
Add Update and Delete products  
Create promotions to discount products  
View customers and orders  
View order status  
View financial/business metrics time series  

Feature List:  
Home Page  
Login Page  
Signup Page  
Products Browse  
Product Details  
View Cart  
Add to Cart  
Suggestions  
Payment Process  
Order History  
Shipping Tracking  
Email Notifications  
Admin Product Management  
Admin Promotions Management  
Admin Customer Management  
Admin Order Management  
Admin Financial Metrics  

Frontend Application Component Architecture  
Main  
|- GlobalStateManager  
|- APIWrapper  
|- Authentication  
|- UI  
  |- Home  
  |- Login  
  |- SignUp  
  |- Products  
  |  |- Browse
  |  |  |- Search Filter and Bucket
  |  |  |- Sort and Paginate
  |  |  |- Promotions
  |  |- Details  
  |     |- AddToCart 
  |     |- Suggestions
  |     |- Promotions
  |     |- Reviews
  |- View Cart
  |  |- Suggestions
  |  |- Checkout
  |     |- Order Summary
  |     |- Create Account OR Login
  |     |- Payment Details
  |     |- Final Invoice
  |     |- Submit Order
  |     |- View Order OR Continue Shopping
  |- Account  
  |  |- Order History  
  |  |  |- View Order  
  |  |  |- Shipping Tracking  
  |  |- Email Preferences
  |  |- Contact Information
  |  |- Credit Cards
  |  |- Billing History
  |  |- Billing Addresses
  |  |- Shipping Addresses
  |  |- Help Desk
  |  |- Returns
  |- Admin  
     |- ProductManagement  
     |- PromotionsManagement  
     |- CustomerManagement  
     |- OrderManagement  
     |  |- Filterable Sales Graph
     |- Help Desk Management
     |  |- Tickets
     |  |- View Ticket
     |  |- Communication History
     |- ReturnsManagement
     |  |- Prepaid Labels
     |  |- Shipment Tracking
     |  |- Return Status Tracking
     |- Email Campaigns
