// Copyright © Spatial Corporation. All rights reserved.

import { NextRequest, NextResponse } from "next/server";

import { RoomInviteOptions } from "@spatial/contracts";
import { email } from "@spatial/server";
import { Invitation } from "@spatial/email";

/**
 * Send an invitation to a room.
 * @param request A request sent to the server.
 */
export const POST = async (request: NextRequest) => {
  const options: RoomInviteOptions = await request.json();

  await email({
    to: options.email,
    subject: "Join the conversation",
    body: <Invitation room={options.room} note={options.note} />,
  });

  return NextResponse.json({});
};
