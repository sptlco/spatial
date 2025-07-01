// Copyright © Spatial Corporation. All rights reserved.

import { QRCodeSVG as Primitive } from "qrcode.react";

import { Element, ElementProps, Node } from "..";

/**
 * Create a new QR code element.
 * @param props Configurable options for the element.
 * @returns A QR code element.
 */
export const QRCode: Element<QRCodeProps> = (props: QRCodeProps): Node => {
  return <Primitive {...props} />;
};

type QRCodeProps = ElementProps & {
  value: string | string[];
  title?: string;
  bgColor?: string;
  fgColor?: string;
  size?: number;
};
