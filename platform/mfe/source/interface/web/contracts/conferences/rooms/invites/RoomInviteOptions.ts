// Copyright © Spatial. All rights reserved.

/**
 * Configurable options for a room invite.
 */
export type RoomInviteOptions = {
  /**
   * A room number.
   */
  room: string;

  /**
   * The invite's recipient.
   */
  email: string;

  /**
   * An optional note.
   */
  note?: string;
};
