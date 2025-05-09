// Copyright © Spatial. All rights reserved.

import { ReactElement } from "react";
import { createTransport } from "nodemailer";
import { render } from "@react-email/components";

import Mail from "nodemailer/lib/mailer";
import SMTPTransport from "nodemailer/lib/smtp-transport";

/**
 * Send an email.
 * @param options Configurable options for the email.
 */
export const email = async (options: EmailOptions) => {
  const transportOptions: SMTPTransport.Options = {
    host: process.env.MAIL_HOST,
    port: Number.parseInt(process.env.MAIL_PORT || "465"),
    secure: true,
    auth: {
      user: process.env.MAIL_USER,
      pass: process.env.MAIL_PASSWORD,
    },
  };

  const mailOptions: Mail.Options = {
    from: `Spatial <${process.env.MAIL_USER}>`,
    to: options.to,
    subject: options.subject,
    html: render(options.body),
  };

  try {
    await createTransport(transportOptions).sendMail(mailOptions);
  } catch (error) {}
};

type EmailOptions = {
  to: string | string[];
  subject: string;
  body: ReactElement;
};
