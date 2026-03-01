// Copyright © Spatial Corporation. All rights reserved.

import { Card, Container, createElement } from "@sptlco/design";

/**
 * A page header element.
 */
export const Header = createElement<typeof Container, { title?: string; description?: string }>((props, ref) => {
  return (
    (props.title || props.description) && (
      <Container {...props} ref={ref} className="flex flex-col px-10">
        {props.title && (
          <Card.Title className="col-span-full text-2xl xl:text-display xl:-translate-x-4 font-extrabold xl:pb-16">{props.title}</Card.Title>
        )}
      </Container>
    )
  );
});
