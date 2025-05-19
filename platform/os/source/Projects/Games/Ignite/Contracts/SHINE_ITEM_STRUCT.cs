// Copyright © Spatial. All rights reserved.

using Ignite.Assets.Types;
using Ignite.Models;
using Spatial.Extensions;
using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing item data.
/// </summary>
public class SHINE_ITEM_STRUCT : ProtocolBuffer
{
    /// <summary>
    /// The item's identification number.
    /// </summary>
    public ushort itemid;

    /// <summary>
    /// The item's attributes.
    /// </summary>
    public SHINE_ITEM_ATTRIBUTE itemattr;

    /// <summary>
    /// Create a new <see cref="SHINE_ITEM_STRUCT"/>.
    /// </summary>
    public SHINE_ITEM_STRUCT() { }

    /// <summary>
    /// Create a new <see cref="SHINE_ITEM_STRUCT"/>.
    /// </summary>
    /// <param name="item">An item identification number.</param>
    public SHINE_ITEM_STRUCT(Item item)
    {
        itemid = item.ItemId;
        itemattr = ItemInfo.Read(itemid).Client.Class switch {
            ItemClassEnum.ITEMCLASS_BYTELOT => new ShineItemAttr_ByteLot {
                lot = (byte) item.Lot
            },
            ItemClassEnum.ITEMCLASS_WORDLOT => new ShineItemAttr_WordLot {
                lot = (ushort) item.Lot
            },
            ItemClassEnum.ITEMCLASS_DWRDLOT => new ShineItemAttr_DwrdLot {
                lot = (uint) item.Lot
            },
            ItemClassEnum.ITEMCLASS_QUESTITEM => new ShineItemAttr_QuestItem {
                lot = (ushort) item.Lot
            },
            ItemClassEnum.ITEMCLASS_AMULET => new ShineItemAttr_Amulet {
                deletetime = item.Expires != null ? new ShineDateTime(item.Expires.Value) : ShineDateTime.MaxValue,
                IsPutOnBelonged = item.Bound ? SHINE_PUT_ON_BELONGED_ITEM.SPOBI_BELONGED : SHINE_PUT_ON_BELONGED_ITEM.SPOBI_NOT_BELONGED,
                upgrade = item.Upgrades,
                strengthen = item.Fortifications,
                upgradefailcount = item.FailedUpgrades,
                UpgradeOption = new ItemOptionStorage {
                    optioninfo = new ItemOptionStorage.FixedInfo {
                        optionnumber = new ItemOptionNumber {
                            identify = false,
                            optionnum = (byte) item.Options.Count
                        }
                    },
                    optionlist = item.UpgradeOptions.ToArray(kvp => new ItemOptionStorage.Element {
                        itemoption_type = (byte) kvp.Key,
                        itemoption_value = (ushort) kvp.Value
                    })
                },
                randomOptionChangedCount = item.Reconfigurations,
                option = new ItemOptionStorage {
                    optioninfo = new ItemOptionStorage.FixedInfo {
                        optionnumber = new ItemOptionNumber {
                            identify = false,
                            optionnum = (byte) item.Options.Count
                        }
                    },
                    optionlist = item.Options.ToArray(kvp => new ItemOptionStorage.Element {
                        itemoption_type = (byte) kvp.Key,
                        itemoption_value = (ushort) kvp.Value
                    })
                }
            },
            ItemClassEnum.ITEMCLASS_WEAPON => new ShineItemAttr_Weapon {
                upgrade = item.Upgrades,
                strengthen = item.Fortifications,
                upgradefailcount = item.FailedUpgrades,
                IsPutOnBelonged = item.Bound ? SHINE_PUT_ON_BELONGED_ITEM.SPOBI_BELONGED : SHINE_PUT_ON_BELONGED_ITEM.SPOBI_NOT_BELONGED,
                mobkills = item.Kills.ToPaddedArray(
                    size: 3,
                    value: ShineItemAttr_Weapon.Kill.Null,
                    selector: kvp => new ShineItemAttr_Weapon.Kill {
                        monster = kvp.Key,
                        killcount = kvp.Value
                    }),
                CharacterTitleMobID = item.TitleMobId,
                usertitle = item.Name,
                gemSockets = item.Sockets.ToPaddedArray(
                    size: 3,
                    value: ShineItemAttr_Weapon.Socket.Null,
                    selector: socket => new ShineItemAttr_Weapon.Socket {
                        elementalGemID = socket.GemId,
                        restCount = socket.Rest
                    }),
                maxSocketCount = item.MaxSockets,
                createdSocketCount = (byte) item.Sockets.Count,
                deletetime = item.Expires != null ? new ShineDateTime(item.Expires.Value) : ShineDateTime.MaxValue,
                randomOptionChangedCount = item.Reconfigurations,
                option = new ItemOptionStorage {
                    optioninfo = new ItemOptionStorage.FixedInfo {
                        optionnumber = new ItemOptionNumber {
                            identify = false,
                            optionnum = (byte) item.Options.Count
                        }
                    },
                    optionlist = item.Options.ToArray(kvp => new ItemOptionStorage.Element {
                        itemoption_type = (byte) kvp.Key,
                        itemoption_value = (ushort) kvp.Value
                    })
                }
            },
            ItemClassEnum.ITEMCLASS_ARMOR => new ShineItemAttr_Armor {
                upgrade = item.Upgrades,
                strengthen = item.Fortifications,
                upgradefailcount = item.FailedUpgrades,
                IsPutOnBelonged = item.Bound ? SHINE_PUT_ON_BELONGED_ITEM.SPOBI_BELONGED : SHINE_PUT_ON_BELONGED_ITEM.SPOBI_NOT_BELONGED,
                deletetime = item.Expires != null ? new ShineDateTime(item.Expires.Value) : ShineDateTime.MaxValue,
                randomOptionChangedCount = item.Reconfigurations,
                option = new ItemOptionStorage {
                    optioninfo = new ItemOptionStorage.FixedInfo {
                        optionnumber = new ItemOptionNumber {
                            identify = false,
                            optionnum = (byte) item.Options.Count
                        }
                    },
                    optionlist = item.Options.ToArray(kvp => new ItemOptionStorage.Element {
                        itemoption_type = (byte) kvp.Key,
                        itemoption_value = (ushort) kvp.Value
                    })
                }
            },
            ItemClassEnum.ITEMCLASS_SHIELD => new ShineItemAttr_Shield {
                upgrade = item.Upgrades,
                strengthen = item.Fortifications,
                upgradefailcount = item.FailedUpgrades,
                IsPutOnBelonged = item.Bound ? SHINE_PUT_ON_BELONGED_ITEM.SPOBI_BELONGED : SHINE_PUT_ON_BELONGED_ITEM.SPOBI_NOT_BELONGED,
                deletetime = item.Expires != null ? new ShineDateTime(item.Expires.Value) : ShineDateTime.MaxValue,
                randomOptionChangedCount = item.Reconfigurations,
                option = new ItemOptionStorage {
                    optioninfo = new ItemOptionStorage.FixedInfo {
                        optionnumber = new ItemOptionNumber {
                            identify = false,
                            optionnum = (byte) item.Options.Count
                        }
                    },
                    optionlist = item.Options.ToArray(kvp => new ItemOptionStorage.Element {
                        itemoption_type = (byte) kvp.Key,
                        itemoption_value = (ushort) kvp.Value
                    })
                }
            },
            ItemClassEnum.ITEMCLASS_BOOT => new ShineItemAttr_Boot {
                upgrade = item.Upgrades,
                strengthen = item.Fortifications,
                upgradefailcount = item.FailedUpgrades,
                IsPutOnBelonged = item.Bound ? SHINE_PUT_ON_BELONGED_ITEM.SPOBI_BELONGED : SHINE_PUT_ON_BELONGED_ITEM.SPOBI_NOT_BELONGED,
                deletetime = item.Expires != null ? new ShineDateTime(item.Expires.Value) : ShineDateTime.MaxValue,
                randomOptionChangedCount = item.Reconfigurations,
                option = new ItemOptionStorage {
                    optioninfo = new ItemOptionStorage.FixedInfo {
                        optionnumber = new ItemOptionNumber {
                            identify = false,
                            optionnum = (byte) item.Options.Count
                        }
                    },
                    optionlist = item.Options.ToArray(kvp => new ItemOptionStorage.Element {
                        itemoption_type = (byte) kvp.Key,
                        itemoption_value = (ushort) kvp.Value
                    })
                }
            },
            ItemClassEnum.ITEMCLASS_FURNITURE => new ShineItemAttr_Furniture {
                flag = new ShineItemAttr_Furniture.Flags {
                    IsPlaced = item.Placed
                },
                furnicherID = item.FurnitureId,
                deletetime = item.Expires != null ? new ShineDateTime(item.Expires.Value) : ShineDateTime.MaxValue,
                LocX = item.Position.X,
                LocY = item.Position.Y,
                LocZ = item.Position.Z,
                Direct = item.Position.Rotation,
                dEndureEndTime = item.Breaks != null ? new ShineDateTime(item.Breaks.Value) : ShineDateTime.MaxValue,
                nEndureGrade = item.Grade,
                nRewardMoney = item.Value
            },
            ItemClassEnum.ITEMCLASS_DECORATION => new ShineItemAttr_Decoration {
                deletetime = item.Expires != null ? new ShineDateTime(item.Expires.Value) : ShineDateTime.MaxValue,
                IsPutOnBelonged = item.Bound ? SHINE_PUT_ON_BELONGED_ITEM.SPOBI_BELONGED : SHINE_PUT_ON_BELONGED_ITEM.SPOBI_NOT_BELONGED,
            },
            ItemClassEnum.ITEMCLASS_SKILLSCROLL => new ShineItemAttr_SkillScroll(),
            ItemClassEnum.ITEMCLASS_RECALLSCROLL => new ShineItemAttr_RecallScroll {
                lot = (byte) item.Lot
            },
            ItemClassEnum.ITEMCLASS_BINDITEM => new ShineItemAttr_BindItem {
                portalnum = (byte) item.Portals.Count,
                portal = item.Portals.ToPaddedArray(
                    size: 10,
                    value: ShineItemAttr_BindItem.Bind.Null,
                    selector: portal => new ShineItemAttr_BindItem.Bind {
                        mapid = portal.MapId,
                        x = (uint) portal.X,
                        y = (uint) portal.Y
                    })
            },
            ItemClassEnum.ITEMCLASS_UPSOURCE => new ShineItemAttr_UpSource {
                lot = (byte) item.Lot
            },
            ItemClassEnum.ITEMCLASS_ITEMCHEST => new ShineItemAttr_ItemChest {
                type = new ShineItemAttr_ItemChest.Type {
                    itemnum = (byte) item.Contents.Length,
                },
                content = item.Contents.ToPaddedArray(
                    size: 8,
                    value: SHINE_ITEM_REGISTNUMBER.Null,
                    selector: key => new SHINE_ITEM_REGISTNUMBER(key))
            },
            ItemClassEnum.ITEMCLASS_WTLICENCE => new ShineItemAttr_Licence(),
            ItemClassEnum.ITEMCLASS_KQ => new ShineItemAttr_KingdomQuest(),
            ItemClassEnum.ITEMCLASS_HOUSESKIN => new ShineItemAttr_MiniHouseSkin {
                deletetime = item.Expires != null ? new ShineDateTime(item.Expires.Value) : ShineDateTime.MaxValue,
            },
            ItemClassEnum.ITEMCLASS_UPRED => new ShineItemAttr_UpRed {
                lot = (byte) item.Lot
            },
            ItemClassEnum.ITEMCLASS_UPBLUE => new ShineItemAttr_UpBlue {
                lot = (byte) item.Lot
            },
            ItemClassEnum.ITEMCLASS_KQSTEP => new ShineItemAttr_KQStep {
                lot = (ushort) item.Lot
            },
            ItemClassEnum.ITEMCLASS_FEED => new ShineItemAttr_Feed {
                lot = (byte) item.Lot
            },
            ItemClassEnum.ITEMCLASS_RIDING => new ShineItemAttr_Riding {
                hungrypoint = item.Hunger,
                deletetime = item.Expires != null ? new ShineDateTime(item.Expires.Value) : ShineDateTime.MaxValue,
                bitflag = new ShineItemAttr_Riding.Flags {
                    ridenum = item.Rides,
                    duringriding = item.Riding
                },
                IsPutOnBelonged = item.Bound ? SHINE_PUT_ON_BELONGED_ITEM.SPOBI_BELONGED : SHINE_PUT_ON_BELONGED_ITEM.SPOBI_NOT_BELONGED,
                nHP = (uint) item.Health,
                nGrade = item.Grade,
                nRareFailCount = item.FailedUpgrades
            },
            ItemClassEnum.ITEMCLASS_AMOUNT => new ShineItemAttr_Amount {
                amount = item.Amount
            },
            ItemClassEnum.ITEMCLASS_UPGOLD => new ShineItemAttr_UpGold {
                lot = (byte) item.Lot
            },
            ItemClassEnum.ITEMCLASS_COSWEAPON => new ShineItemAttr_CostumWeapon {
                CostumCharged = item.Charged != null ? new ShineDateTime(item.Charged.Value) : ShineDateTime.MaxValue
            },
            ItemClassEnum.ITEMCLASS_ACTIONITEM => new ShineItemAttr_ActionItem {
                deletetime = item.Expires != null ? new ShineDateTime(item.Expires.Value) : ShineDateTime.MaxValue,
            },
            ItemClassEnum.ITEMCLASS_GBCOIN => new ShineItemAttr_GBCoin(),
            ItemClassEnum.ITEMCLASS_CAPSULE => new ShineItemAttr_Capsule {
                content = item.Contents[0],
                useabletime = item.Useable != null ? new ShineDateTime(item.Useable.Value) : ShineDateTime.MaxValue
            },
            ItemClassEnum.ITEMCLASS_CLOSEDCARD => new ShineItemAttr_MobCardCollect_Unident {
                SerialNumber = item.Serial,
                CardID = item.Card,
                Star = item.Stars,
                Group = item.Group
            },
            ItemClassEnum.ITEMCLASS_OPENCARD => new ShineItemAttr_MobCardCollect {
                SerialNumber = item.Serial,
                Star = item.Stars
            },
            ItemClassEnum.ITEMCLASS_NOEFFECT => new ShineItemAttr_NoEffect {
                lot = (byte) item.Lot
            },
            ItemClassEnum.ITEMCLASS_ENCHANT => new ShineItemAttr_Enchant {
                lot = (byte) item.Lot
            },
            ItemClassEnum.ITEMCLASS_ACTIVESKILL => new ShineItemAttr_ActiveSkill {
                lot = (ushort) item.Lot
            },
            ItemClassEnum.ITEMCLASS_PUP => new ShineItemAttr_Pet {
                nPetRegNum = item.PetRID,
                nPetID = item.PetId,
                sName = item.Name,
                bSummoning = item.Summoning
            },
            ItemClassEnum.ITEMCLASS_COSSHIELD => new ShineItemAttr_CostumShield {
                CostumCharged = item.Charged != null ? new ShineDateTime(item.Charged.Value) : ShineDateTime.MaxValue
            },
            ItemClassEnum.ITEMCLASS_BRACELET => new ShineItemAttr_Bracelet {
                deletetime = item.Expires != null ? new ShineDateTime(item.Expires.Value) : ShineDateTime.MaxValue,
                IsPutOnBelonged = item.Bound ? SHINE_PUT_ON_BELONGED_ITEM.SPOBI_BELONGED : SHINE_PUT_ON_BELONGED_ITEM.SPOBI_NOT_BELONGED,
                upgrade = item.Upgrades,
                strengthen = item.Fortifications,
                upgradefailcount = item.FailedUpgrades,
                randomOptionChangedCount = item.Reconfigurations,
                option = new ItemOptionStorage {
                    optioninfo = new ItemOptionStorage.FixedInfo {
                        optionnumber = new ItemOptionNumber {
                            identify = false,
                            optionnum = (byte) item.Options.Count
                        }
                    },
                    optionlist = item.Options.ToArray(kvp => new ItemOptionStorage.Element {
                        itemoption_type = (byte) kvp.Key,
                        itemoption_value = (ushort) kvp.Value
                    })
                }
            },
            _ => throw new System.NotImplementedException(),
        };
    }

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        itemattr = ItemInfo.Read(itemid = ReadUInt16()).Client.Class switch {
            ItemClassEnum.ITEMCLASS_BYTELOT => Read<ShineItemAttr_ByteLot>(),
            ItemClassEnum.ITEMCLASS_WORDLOT => Read<ShineItemAttr_WordLot>(),
            ItemClassEnum.ITEMCLASS_DWRDLOT => Read<ShineItemAttr_DwrdLot>(),
            ItemClassEnum.ITEMCLASS_QUESTITEM => Read<ShineItemAttr_QuestItem>(),
            ItemClassEnum.ITEMCLASS_AMULET => Read<ShineItemAttr_Amulet>(),
            ItemClassEnum.ITEMCLASS_WEAPON => Read<ShineItemAttr_Weapon>(),
            ItemClassEnum.ITEMCLASS_ARMOR => Read<ShineItemAttr_Armor>(),
            ItemClassEnum.ITEMCLASS_SHIELD => Read<ShineItemAttr_Shield>(),
            ItemClassEnum.ITEMCLASS_BOOT => Read<ShineItemAttr_Boot>(),
            ItemClassEnum.ITEMCLASS_FURNITURE => Read<ShineItemAttr_Furniture>(),
            ItemClassEnum.ITEMCLASS_DECORATION => Read<ShineItemAttr_Decoration>(),
            ItemClassEnum.ITEMCLASS_SKILLSCROLL => Read<ShineItemAttr_SkillScroll>(),
            ItemClassEnum.ITEMCLASS_RECALLSCROLL => Read<ShineItemAttr_RecallScroll>(),
            ItemClassEnum.ITEMCLASS_BINDITEM => Read<ShineItemAttr_BindItem>(),
            ItemClassEnum.ITEMCLASS_UPSOURCE => Read<ShineItemAttr_UpSource>(),
            ItemClassEnum.ITEMCLASS_ITEMCHEST => Read<ShineItemAttr_ItemChest>(),
            ItemClassEnum.ITEMCLASS_WTLICENCE => Read<ShineItemAttr_Licence>(),
            ItemClassEnum.ITEMCLASS_KQ => Read<ShineItemAttr_KingdomQuest>(),
            ItemClassEnum.ITEMCLASS_HOUSESKIN => Read<ShineItemAttr_MiniHouseSkin>(),
            ItemClassEnum.ITEMCLASS_UPRED => Read<ShineItemAttr_UpRed>(),
            ItemClassEnum.ITEMCLASS_UPBLUE => Read<ShineItemAttr_UpBlue>(),
            ItemClassEnum.ITEMCLASS_KQSTEP => Read<ShineItemAttr_KQStep>(),
            ItemClassEnum.ITEMCLASS_FEED => Read<ShineItemAttr_Feed>(),
            ItemClassEnum.ITEMCLASS_RIDING => Read<ShineItemAttr_Riding>(),
            ItemClassEnum.ITEMCLASS_AMOUNT => Read<ShineItemAttr_Amount>(),
            ItemClassEnum.ITEMCLASS_UPGOLD => Read<ShineItemAttr_UpGold>(),
            ItemClassEnum.ITEMCLASS_COSWEAPON => Read<ShineItemAttr_CostumWeapon>(),
            ItemClassEnum.ITEMCLASS_ACTIONITEM => Read<ShineItemAttr_ActionItem>(),
            ItemClassEnum.ITEMCLASS_GBCOIN => Read<ShineItemAttr_GBCoin>(),
            ItemClassEnum.ITEMCLASS_CAPSULE => Read<ShineItemAttr_Capsule>(),
            ItemClassEnum.ITEMCLASS_CLOSEDCARD => Read<ShineItemAttr_MobCardCollect_Unident>(),
            ItemClassEnum.ITEMCLASS_OPENCARD => Read<ShineItemAttr_MobCardCollect>(),
            ItemClassEnum.ITEMCLASS_NOEFFECT => Read<ShineItemAttr_NoEffect>(),
            ItemClassEnum.ITEMCLASS_ENCHANT => Read<ShineItemAttr_Enchant>(),
            ItemClassEnum.ITEMCLASS_ACTIVESKILL => Read<ShineItemAttr_ActiveSkill>(),
            ItemClassEnum.ITEMCLASS_PUP => Read<ShineItemAttr_Pet>(),
            ItemClassEnum.ITEMCLASS_COSSHIELD => Read<ShineItemAttr_CostumShield>(),
            ItemClassEnum.ITEMCLASS_BRACELET => Read<ShineItemAttr_Bracelet>(),
            _ => throw new System.NotImplementedException()
        };
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(itemid);
        Write(itemattr);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        itemattr.Dispose();

        base.Dispose();
    }
}
