// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";
import { clsx } from "clsx";

/**
 * An element displaying data in a tabular format.
 */
export const Table = {
  /**
   * Contains all the parts of a table.
   */
  Root: createElement<"table">((props, ref) => <table {...props} ref={ref} />),

  /**
   * Contains the body content in a table.
   */
  Body: createElement<"tbody">((props, ref) => <tbody {...props} ref={ref} />),

  /**
   * A row in a table.
   */
  Row: createElement<"tr">((props, ref) => <tr {...props} ref={ref} />),

  /**
   * Groups table header data.
   */
  Header: createElement<"thead">((props, ref) => <thead {...props} ref={ref} />),

  /**
   * A standard data cell in a table.
   */
  Cell: createElement<"td">((props, ref) => <td {...props} ref={ref} className={clsx("border-b border-line-subtle pb-10", props.className)} />),

  /**
   * A header cell in a table.
   */
  Column: createElement<"th">((props, ref) => <th {...props} ref={ref} className={clsx("border-b border-line-subtle pb-10", props.className)} />),

  /**
   * Groups footer content in a table.
   */
  Footer: createElement<"tfoot">((props, ref) => <tfoot {...props} ref={ref} />)
};
