﻿using AutoMapper;
using DataAccess.Entities;
using DataAccess.Models.BranModel;
using DataAccess.Models.CategoryModel;
using DataAccess.Models.ChannelCrawlModel;
using DataAccess.Models.ChannelRecordModel;
using DataAccess.Models.HashtagModel;
using DataAccess.Models.LocationModel;
using DataAccess.Models.LoginUser;
using DataAccess.Models.OrganizationModel;
using DataAccess.Models.PlatFormModel;
using DataAccess.Models.PostCrawlModel;
using DataAccess.Models.ReactionModel;
using DataAccess.Models.ReactionTypeModel;
using DataAccess.Models.Role;
using DataAccess.Models.WalletModel;

namespace Business.Utils
{
    public static class MapperConfig
    {
        public static IMapper GetMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Role, RoleDTO>().ReverseMap();
                cfg.CreateMap<Role, InsertRoleDTO>().ReverseMap();
                cfg.CreateMap<Role, UpdateRoleDTO>().ReverseMap();
                cfg.CreateMap<User, LoginUserDTO>().ReverseMap();
                cfg.CreateMap<User, InsertUserDTO>().ReverseMap();
                cfg.CreateMap<User, FacebookLoginDto>().ReverseMap();
                cfg.CreateMap<User, GoogleLoginDto>().ReverseMap();
                cfg.CreateMap<User, GoogleSignUpDto>().ReverseMap();
                cfg.CreateMap<User, FacebookSignUpDto>().ReverseMap();
                cfg.CreateMap<User, UpdateUserDto>().ReverseMap();
                cfg.CreateMap<User, UpdateUserPassworDto>().ReverseMap();

                cfg.CreateMap<Organization, InsertOrganizationDto>().ReverseMap();
                cfg.CreateMap<Organization, OrganizationDto>().ReverseMap();
                cfg.CreateMap<Organization, UpdateOrganizationDto>().ReverseMap();

                cfg.CreateMap<Brand, BrandDto>().ReverseMap();
                cfg.CreateMap<Brand, InsertBrandDto>().ReverseMap();
                cfg.CreateMap<Brand, UpdateBrandDto>().ReverseMap();

                cfg.CreateMap<Platform, PlatformDto>().ReverseMap();
                cfg.CreateMap<Platform, InsertPlatformDto>().ReverseMap();
                cfg.CreateMap<Platform, UpdatePlatformDto>().ReverseMap();

                cfg.CreateMap<Category, CategoryDto>().ReverseMap();
                cfg.CreateMap<Category, InsertCategoryDto>().ReverseMap();
                cfg.CreateMap<Category, UpdateCategoryDto>().ReverseMap();

                cfg.CreateMap<Reactiontype, ReactionTypeDto>().ReverseMap();
                cfg.CreateMap<Reactiontype, InsertReactionType>().ReverseMap();
                cfg.CreateMap<Reactiontype, UpdateReactionTypeDto>().ReverseMap();

                cfg.CreateMap<Reaction, ReactionDto>().ReverseMap();
                cfg.CreateMap<Reaction, InsertReactionDto>().ReverseMap();
                cfg.CreateMap<Reaction, UpdateReactionDto>().ReverseMap();

                cfg.CreateMap<Hashtag, HashtagDto>().ReverseMap();
                cfg.CreateMap<Hashtag, InsertHashtagDto>().ReverseMap();
                cfg.CreateMap<Hashtag, UpdateHashtagDto>().ReverseMap();

               
                cfg.CreateMap<Location, LocationDto>().ReverseMap();
                cfg.CreateMap<Location, InsertLocationDto>().ReverseMap();
                cfg.CreateMap<Location, UpdateLocationDto>().ReverseMap();

                cfg.CreateMap<ChannelCrawl, ChannelCrawlDto>().ReverseMap();
                cfg.CreateMap<ChannelCrawl, InsertChannelCrawlDto>().ReverseMap();
                cfg.CreateMap<ChannelCrawl, UpdateChannelCrawlDto>().ReverseMap();
                cfg.CreateMap<ChannelCategory, ChannelCategoryDto>().ReverseMap();


                cfg.CreateMap<ChannelRecord, ChannelRecordDto>().ReverseMap();
                cfg.CreateMap<ChannelRecord, InsertChannelRecordDto>().ReverseMap();
                cfg.CreateMap<ChannelRecord, UpdateChannelRecordDto>().ReverseMap();

                cfg.CreateMap<PostCrawl, PostCrawlDto>().ReverseMap();
                cfg.CreateMap<PostCrawl, InsertPostCrawlDto>().ReverseMap();
                cfg.CreateMap<PostCrawl, UpdatePostCrawlDto>().ReverseMap();


                cfg.CreateMap<Wallet, WalletDto>().ReverseMap();
                cfg.CreateMap<Wallet, InsertWalletDto>().ReverseMap();
                cfg.CreateMap<Wallet, UpdateWalletDto>().ReverseMap();


            });
            return configuration.CreateMapper();
        }
    }
}
