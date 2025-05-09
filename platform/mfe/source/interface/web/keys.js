// Copyright © Spatial. All rights reserved.

import { generateKeyPairSync } from "crypto";

const { publicKey, privateKey } = generateKeyPairSync("rsa", {
  modulusLength: 4096,
  publicKeyEncoding: { type: "spki", format: "pem" },
  privateKeyEncoding: { type: "pkcs8", format: "pem" },
});

const privateBase64 = Buffer.from(privateKey).toString("base64");
const publicBase64 = Buffer.from(publicKey).toString("base64");

console.log("PRIVATE KEY (Base64):\n", privateBase64);
console.log("\nPUBLIC KEY (Base64):\n", publicBase64);
