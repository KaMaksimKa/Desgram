# Desgram
1)Первое что хочется пояснить почему я использую двойной маппинг в таких моментах как:
```C#
    var following = await _context.UserSubscriptions.AsNoTracking()
        .Where(s => s.ContentMakerId == user.Id && s.DeletedDate == null && s.IsApproved)
        .Select(s => s.Follower)
        .Skip(model.Skip).Take(model.Take)
        .ProjectTo<PartialUserModel>(_mapper.ConfigurationProvider)
        .ToListAsync();

    return following.Select(u => _mapper.Map<PartialUserModel>(u)).ToList();
```
и еще


Здесь сначала используется ProjectTo для проекции User в PartialUserModel, а потом mapper.Map() для PartialUserModel в PartialUserModel. Это зделано так, потому что
во многих случиях при маппинге Post в PostModel или User в UserModel и если использовать mapper.Map(), то для подчета количества, например, постов придется писать Include, что в свою очередь загрузит все посты пользователя только для подчета их количества, я посчитал что это неприятно, поэтому я использою сначала ProjectTo, чтобы достать все неоходимое малой кровью, а mapper.Map() для того чтобы в рантайме получить url для аттачей которые содержатся в моделях. Лучшего решения я не придумал.

2) Также где необходимо прокинуть в мапинг дополнительные аргументы, такие как requestorId, я не использую Automapper, так как туда невозможно прокинуть доп. аргументы, а использую CustomMapperService, где я в ручную составляю маппинг.
3) В данный момент содержимым контента поста могут быть только фото, но в дальнейшем это планируется расширить видео, не добавил сейчас, из-за сложности и не понимания как нужно хранить и загружать видео(я так понимаю не одним большим файлом, а разбивать на более мелкие).
4) В интерфейсах служб есть комментарии, что делают методы
