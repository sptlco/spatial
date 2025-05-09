// Copyright © Spatial. All rights reserved.

import { clsx } from "clsx";

import {
  Body,
  Button,
  Element,
  ElementProps,
  Email,
  Footer,
  Header,
  Link,
  Node,
  Text,
} from "..";

/**
 * Create a new invitation email element.
 * @param props Configurable options for the element.
 * @returns An invitation email element.
 */
export const Invitation: Element<InvitationProps> = (
  props: InvitationProps,
): Node => {
  return (
    <Email preview={`You were invited to room ${props.room?.toUpperCase()}`}>
      <Body>
        <Header />
        <Text className="p-2u text-m !pt-0">
          Greetings,
          <br />
          <br />
          You were invited to a conference room by a user of our platform.
          Conferences are peer-to-peer (P2P) video calls designed for simplicity
          and security. Enter the following room number or{" "}
          <Link
            href={`${process.env.NEXT_PUBLIC_BASE_URL}/conference?room=${props.room}`}
          >
            click this link
          </Link>{" "}
          to join the conversation.
        </Text>
        <Text
          children={props.room || "00000000"}
          className={clsx(
            "text-center text-3xl font-bold uppercase md:text-5xl",
          )}
        />
        {props.note && (
          <Text className="p-2u text-s text-foreground-secondary !pb-0 text-center italic">
            "{props.note}"
          </Text>
        )}
        <Footer />
      </Body>
    </Email>
  );
};

type InvitationProps = ElementProps & {
  room: string;
  note?: string;
};

export default Invitation;
