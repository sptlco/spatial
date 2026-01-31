// Copyright © Spatial Corporation. All rights reserved.

import { Account, CreateAccountOptions } from "@sptlco/data";
import { Controller } from "..";

export class AccountController extends Controller {
  /**
   * Create a new account.
   * @param options Configurable options for the account.
   * @returns An account.
   */
  public create = async (options: CreateAccountOptions) => {
    return this.post<Account>("accounts", options);
  };

  /**
   * Update an account.
   * @param account The account to update.
   * @returns The updated account.
   */
  public update = async (account: Account) => {
    return this.patch<Account>("accounts", account);
  };

  /**
   * Delete an account.
   * @param id The account to delete.
   * @returns The response from the server.
   */
  public del = async (id: string) => {
    return this.delete(`accounts/${id}`);
  };
}
