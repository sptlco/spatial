// Copyright © Spatial Corporation. All rights reserved.

"use client";

import axios from "axios";
import clsx from "clsx";
import { useSearchParams } from "next/navigation";
import Peer, { DataConnection, MediaConnection, PeerJSOption } from "peerjs";
import { FormEvent, Suspense, useEffect, useRef, useState } from "react";
import { PackedGrid } from "react-packed-grid";

import { RoomInviteOptions } from "@spatial/contracts";
import { useToast } from "@spatial/hooks";

import {
  A,
  Button,
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
  Div,
  Element,
  ElementProps,
  Form,
  Icon,
  Input,
  Node,
  OTP,
  OTPGroup,
  OTPSlot,
  Page,
  Span,
  Spinner,
  Symbol,
  TextArea,
  ToggleGroup,
  ToggleGroupItem,
} from "@spatial/elements";

/**
 * Create a new conference page element.
 * @returns A conference page element.
 */
export default function ConferencePage() {
  return (
    <Suspense>
      <Conference />
    </Suspense>
  );
}

const Conference = (): Node => {
  const { toast } = useToast();

  const params = useSearchParams().get("room");
  const meeting = useRef<Meeting>(null);

  const [room, setRoom] = useState("");
  const [participants, setParticipants] = useState<Participant[]>([]);

  const reset = async (room: string | null = null) => {
    meeting.current?.disconnect();

    setRoom("");

    await (meeting.current = new Meeting(setParticipants, (e) => {
      toast({
        icon: "emergency_home",
        severity: "danger",
        title: "An unexpected error occurred",
        description: "A connection could not be established. Please try again.",
      });
    })).initialize();

    if (room) {
      meeting.current?.connect(room);
    }

    setRoom(meeting.current.room);
  };

  useEffect(() => {
    window.addEventListener("beforeunload", function (e) {
      meeting.current?.disconnect();
    });
  }, []);

  useEffect(() => {
    reset(params);
    return () => meeting.current?.disconnect();
  }, [params]);

  const render = (): Node => {
    if (!room || !meeting.current?.output) {
      return <Spinner className="size-3/2u" />;
    }

    return (
      <>
        <Grid output={meeting.current.output} participants={participants} />
        <Controls
          onAudioChange={meeting.current.audio}
          onVideoChange={meeting.current.video}
          onReset={reset}
        />
      </>
    );
  };

  return (
    <Page className="dark items-center justify-center">
      <Div className="px-2u py-3/2u space-x-2u fixed left-0 top-0 z-20 flex w-full items-center">
        <Symbol className="h-1u !mr-auto shrink-0" />
        <Span className="space-x-1/2u flex items-center">
          <Join onJoin={reset} />
          <Invite room={room} />
        </Span>
      </Div>
      {render()}
    </Page>
  );
};

const Join: Element<{ onJoin: (room: string) => void } & ElementProps> = ({
  onJoin,
}): Node => {
  const [room, setRoom] = useState("");
  const [open, setOpen] = useState(false);

  const join = () => {
    setOpen(false);
    setRoom("");

    onJoin(room.toUpperCase());
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button size="small" roundness="pill" className="font-bold uppercase">
          <Icon icon="group_add" className="!text-l !leading-none" />
          <Span>Join</Span>
        </Button>
      </DialogTrigger>
      <DialogContent className="dark">
        <DialogHeader>
          <DialogTitle>Join a room</DialogTitle>
          <DialogDescription>Enter a conference room number.</DialogDescription>
        </DialogHeader>
        <OTPGroup>
          <OTP
            autoFocus
            maxLength={8}
            onChange={setRoom}
            className="items-center"
          >
            {[...Array(8)].map((_, i) => (
              <OTPSlot key={i} index={i} className="my-1u" />
            ))}
          </OTP>
        </OTPGroup>
        <DialogFooter>
          <Button
            direction="forward"
            intent="success"
            className="w-full"
            children="Join"
            disabled={room.length < 8}
            onClick={join}
          />
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};

const Invite: Element<{ room: string } & ElementProps> = ({ room }): Node => {
  const { toast } = useToast();

  const [email, setEmail] = useState("");
  const [note, setNote] = useState("");
  const [sending, setSending] = useState(false);

  const copy = () => {
    navigator.clipboard.writeText(
      `${process.env.NEXT_PUBLIC_BASE_URL}/conference?room=${room.toUpperCase()}`,
    );

    toast({
      icon: "content_paste",
      title: "Share this room",
      description: "This room's join link was copied to your clipboard.",
    });
  };

  const send = async (e: FormEvent) => {
    e.preventDefault();

    setSending(true);

    try {
      const options: RoomInviteOptions = {
        room,
        email,
        note,
      };

      await axios.post("/api/conferences/rooms/invites/send", options);

      toast({
        icon: "mail",
        title: "Invite sent",
        description: (
          <>
            A room invitation was sent to <b>{email}</b>.
          </>
        ),
      });
    } catch {
      toast({
        severity: "danger",
        icon: "emergency_home",
        title: "An unexpected error occurred",
        description: "Something went wrong while sending the invite.",
      });
    }

    setSending(false);
  };

  return (
    <Dialog
      onOpenChange={(open) => {
        if (!open) {
          setEmail("");
          setNote("");
        }
      }}
    >
      <DialogTrigger asChild>
        <Button size="small" roundness="pill" className="font-bold uppercase">
          <Icon icon="person_add" className="!text-l !leading-none" />
          <Span>Invite</Span>
        </Button>
      </DialogTrigger>
      <DialogContent className="dark">
        <DialogHeader>
          <DialogTitle>Invite someone</DialogTitle>
          <DialogDescription>Share this room with others.</DialogDescription>
        </DialogHeader>
        <Form className="items-center" onSubmit={send}>
          <Span className="space-x-1/2u pb-1u flex items-center">
            <Span className="text-center text-2xl font-bold uppercase">
              {room || <Spinner className="size-1u" />}
            </Span>
            <A onClick={copy}>
              <Icon icon="content_copy" />
            </A>
          </Span>
          <Input
            autoFocus
            placeholder="Enter an email address"
            value={email}
            onChange={setEmail}
          />
          <TextArea
            placeholder="Add a note (optional)"
            value={note}
            onChange={setNote}
          />
          <DialogFooter className="w-full">
            <Button
              type="submit"
              direction="forward"
              intent="success"
              className="w-full"
              children="Invite"
              disabled={!email || sending}
              loading={sending}
            />
          </DialogFooter>
        </Form>
      </DialogContent>
    </Dialog>
  );
};

const Grid: Element<
  { output?: MediaStream; participants: Participant[] } & ElementProps
> = ({ output, participants }) => {
  const ref = useRef<HTMLVideoElement>(null);

  useEffect(() => {
    if (ref.current && output) {
      ref.current.srcObject = output;
    }
  }, [output]);

  return (
    <>
      <video
        ref={ref}
        autoPlay
        playsInline
        muted
        className="fixed left-0 top-0 h-screen w-screen place-content-center object-cover blur-2xl"
      />
      <PackedGrid
        className="bg-background-overlay z-10 h-screen w-screen"
        boxClassName="relative p-1/2u items-center justify-center"
      >
        {participants.map((participant) => (
          <Tile key={participant.id} participant={participant} />
        ))}
      </PackedGrid>
    </>
  );
};

const Tile: Element<{ participant: Participant } & ElementProps> = ({
  participant,
}): Node => {
  const ref = useRef<HTMLVideoElement>(null);

  useEffect(() => {
    if (ref.current && participant.stream) {
      ref.current.srcObject = participant.stream;
    }
  }, [participant.stream]);

  if (!participant.stream) {
    return (
      <Div className="z-10 grid size-full items-center justify-center">
        <Spinner className="size-3/2u" />
      </Div>
    );
  }

  return (
    <>
      <video
        ref={ref}
        autoPlay
        playsInline
        muted={participant.local}
        className="rounded-1/2u z-10 grid size-full object-cover"
      />
      <Div className="top-2u left-2u space-x-1/2u absolute flex items-center">
        {!participant.audio && (
          <Span
            className={clsx(
              "flex items-center justify-center",
              "size-2u bg-background-severe-danger rounded-full",
            )}
          >
            <Icon icon="mic_off" className="!text-l" />
          </Span>
        )}
        {!participant.video && (
          <Span
            className={clsx(
              "flex items-center justify-center",
              "size-2u bg-background-severe-danger rounded-full",
            )}
          >
            <Icon icon="video_camera_front_off" className="!text-l" />
          </Span>
        )}
      </Div>
      {participant.local && (
        <Span
          children="You"
          className="px-1u py-1/4u rounded-1/2u bg-base-black-9 text-s top-2u right-2u absolute"
        />
      )}
    </>
  );
};

const Controls: Element<
  {
    onAudioChange: (enabled: boolean) => void;
    onVideoChange: (enabled: boolean) => void;
    onReset: () => void;
  } & ElementProps
> = ({ onAudioChange, onVideoChange, onReset }): Node => {
  const [controls, setControls] = useState<string[]>(["audio", "video"]);

  const muted = () => !controls.includes("audio");
  const hidden = () => !controls.includes("video");

  const toggle = (update: string[]) => {
    setControls(update);

    onAudioChange(update.includes("audio"));
    onVideoChange(update.includes("video"));
  };

  return (
    <Div className="bottom-2u space-x-3/4u fixed z-20 mx-auto flex items-center">
      <ToggleGroup
        type="multiple"
        className="space-x-3/4u"
        value={controls}
        onChange={toggle}
      >
        <ToggleGroupItem
          value="audio"
          className={clsx("!size-3u bg-background-primary rounded-full", {
            "!bg-background-severe-danger": muted(),
          })}
        >
          <Icon icon={muted() ? "mic_off" : "mic"} />
        </ToggleGroupItem>
        <ToggleGroupItem
          value="video"
          className={clsx("!size-3u bg-background-primary rounded-full", {
            "!bg-background-severe-danger": hidden(),
          })}
        >
          <Icon
            icon={hidden() ? "video_camera_front_off" : "video_camera_front"}
          />
        </ToggleGroupItem>
      </ToggleGroup>
      <Button
        intent="danger"
        onClick={() => onReset()}
        className="!size-3u !max-h-3u !min-w-0 rounded-full"
      >
        <Icon icon="restart_alt" />
      </Button>
    </Div>
  );
};

/**
 * A user participating in a meeting.
 */
type Participant = {
  /**
   * The user's identifier.
   */
  id: string;

  /**
   * Whether or not the participant is the local user.
   */
  local: boolean;

  /**
   * The user's output stream.
   */
  stream?: MediaStream | null;

  /**
   * Whether or not the user's audio is enabled.
   */
  audio?: boolean | null;

  /**
   * Whether or not the user's video is enabled.
   */
  video?: boolean | null;
};

/**
 * A conference room meeting.
 */
class Meeting {
  private peer: Peer;
  private connections = new Map<string, DataConnection>();

  /**
   * The local output stream.
   */
  public output?: MediaStream;

  /**
   * The meeting's conference room number.
   */
  public room: string;

  /**
   * The participants of the meeting.
   */
  public participants = new Map<string, Participant>();

  /**
   * Create a new meeting.
   * @param onUpdate A callback function fired when participants are updated.
   * @param onError A callback function fired when an error occurs.
   */
  constructor(
    private onUpdate: (participants: Participant[]) => void,
    private onError: (error: string) => void,
  ) {
    this.peer = new Peer((this.room = this.createRoom()), this.createOptions());
  }

  /**
   * Initialize the meeting.
   */
  public initialize = async (audio: boolean = true, video: boolean = true) => {
    try {
      this.output = await navigator.mediaDevices.getUserMedia({
        audio,
        video,
      });

      this.add({
        id: this.peer.id,
        stream: this.output,
        local: true,
        audio,
        video,
      });

      this.peer.on("connection", this.accept);
      this.peer.on("call", this.answer);
      this.peer.on("error", (error) => this.onError(error.message));
    } catch (error: any) {
      this.onError(error.message);
    }
  };

  /**
   * Connect to a meeting room.
   * @param room The room to connect to.
   */
  public connect = (room: string) => {
    this.resolve(this.peer.connect(room));
  };

  /**
   * Disconnect from the meeting.
   */
  public disconnect = () => {
    this.peer.destroy();
    this.connections.forEach((connection) => connection.close());
    this.output?.getTracks().forEach((track) => track.stop());
  };

  /**
   * Configure the user's audio.
   * @param enabled Whether or not audio is enabled.
   */
  public audio = (enabled: boolean) => {
    if (this.output) {
      this.output.getAudioTracks().forEach((t) => (t.enabled = enabled));
      this.broadcast(this.peer.id, { audio: enabled });
    }
  };

  /**
   * Configure the user's video.
   * @param enabled Whether or not video is enabled.
   */
  public video = (enabled: boolean) => {
    if (this.output) {
      this.output.getVideoTracks().forEach((t) => (t.enabled = enabled));
      this.broadcast(this.peer.id, { video: enabled });
    }
  };

  private accept = (connection: DataConnection) => {
    if (this.connections.has(connection.peer)) {
      connection.close();
      return;
    }

    this.resolve(connection);
  };

  private resolve = (connection: DataConnection) => {
    this.connections.get(connection.peer)?.close();
    this.connections.set(connection.peer, connection);

    connection.on("open", () => {
      this.add({
        id: connection.peer,
        local: false,
      });

      if (this.host) {
        connection.send({
          type: "list",
          list: Array.from(this.participants.keys()).filter(
            (peer) => peer !== this.peer.id && peer !== connection.peer,
          ),
        });
      }

      this.stream(this.peer.call(connection.peer, this.output!));
    });

    connection.on("data", this.handle);
    connection.on("error", (error) => this.onError(error.message));
    connection.on("close", () => {
      this.connections.delete(connection.peer);
      this.remove(connection.peer);
    });
  };

  private answer = (call: MediaConnection) => {
    call.answer(this.output);
    this.stream(call);
  };

  private stream = (call: MediaConnection) => {
    call.on("stream", (stream) => {
      this.update(call.peer, {
        stream,
        audio: stream.getAudioTracks().some((track) => track.enabled),
        video: stream.getVideoTracks().some((track) => track.enabled),
      });
    });

    call.on("error", (error) => this.onError(error.message));
    call.on("close", () => {
      this.update(call.peer, { stream: null, audio: null, video: null });
    });
  };

  private handle = (data: any) => {
    switch (data.type) {
      case "list":
        data.list.forEach((peer: string) => {
          if (!this.connections.has(peer)) {
            this.resolve(this.peer.connect(peer));
          }
        });

        break;
      case "update":
        this.update(data.peer, data.update);
        break;
    }
  };

  private add = (participant: Participant) => {
    this.participants.set(participant.id, participant);
    this.propagate();
  };

  private remove = (peer: string) => {
    this.participants.delete(peer);
    this.propagate();
  };

  private broadcast = (peer: string, update: Partial<Participant>) => {
    this.update(peer, update);

    this.connections.forEach((connection) => {
      connection.send({ type: "update", peer, update });
    });
  };

  private update = (peer: string, update: Partial<Participant>) => {
    const participant = this.participants.get(peer);

    if (participant) {
      this.participants.set(peer, { ...participant, ...update });
      this.propagate();
    }
  };

  private propagate = () => {
    this.onUpdate([...this.participants.values()]);
  };

  private get host() {
    return this.peer.id === this.room;
  }

  private createOptions = (): PeerJSOption => {
    return {
      host: "peer.sptlco.com",
      config: {
        iceServers: [
          { urls: "stun:stun.l.google.com:19302" },
          { urls: "stun:stun1.l.google.com:19302" },
          { urls: "stun:stun2.l.google.com:19302" },
        ],
      },
    };
  };

  private createRoom = (): string => {
    return Math.random().toString(36).substring(2, 10).toUpperCase();
  };
}
