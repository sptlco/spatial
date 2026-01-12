// Copyright © Spatial Corporation. All rights reserved.

import { Account } from "@/accounts";
import { Principal } from "@/principals";
import { Session } from "@/sessions";

/**
 * Contains data for a registered user.
 */
export type User = {
  /**
   * The user's registered account.
   */
  account: Account;

  /**
   * The user's authorization principal.
   */
  principal: Principal;

  /**
   * The user's current session.
   */
  session: Session;
};
