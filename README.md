# Amazon Web Services (AWS) Simple Email Service (SES) SMTP Credential Generator

## Project Description
This utility converts an Amazon Web Services (AWS) Identity and Access Management (IAM) user Access Key secret key into an SES SMTP password.

## What does this utility do?
This utility uses your IAM user secret key to create a signing hash for sending raw email via SES. This signing token will allow you to relay email through SES with the format specified by you at the time of sending. It does not store or otherwise send your credentials anywhere and is completely safe to use.

## Why Would I Use/Need this utility?
This utility is especially handy if you've just created your own IAM user without generating it through the SES credential section of the AWS Console. If you have created your own IAM user that's totally fine. Be sure you gave it the necessary permissions.

~~~~
"Statement": [{
      "Effect":"Allow",
      "Action":"ses:SendRawEmail",
      "Resource":"*"
}]
~~~~

## Don't I use the IAM username and password for my SES user?
No, your SES user's username and any manually generated password is not used for SES authentication.

## Don't I just use my IAM user's Access and Secret Key?
Not quite, your SES Access Key is used as the username however the related Secret Key is not used as is for this purpose.

## What credentials do I use to authenticate to SES?
You IAM user's Access Key will be the username and a signing hash for sending RAW email messages that is derived from your IAM user's Secret Key.

## Can't I just get my SMTP crednetials from the AWS console?
Yes but it is easy to misunderstand what credentials are what and often causes confusion.

## What is the SendRAWEmail vs SendEmail permission for SES in IAM?
The SendEmail permission enables a user to provide input via the API that will be used by SES to construct an email message where as the SendRAWEmail permission enables a user to simply relay an already for mated email message. In both cases email messages must comply with (RFC 5322).
